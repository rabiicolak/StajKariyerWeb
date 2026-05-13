using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajKariyerWeb.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        private static readonly Dictionary<string, List<string>> _conceptMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Yapay Zeka", new List<string> { "python", "machine learning", "ai", "yapay zeka", "veri bilimi", "tensorflow", "pytorch", "deep learning", "nlp" } },
            { "Veri Bilimi", new List<string> { "python", "sql", "veri analizi", "data analysis", "pandas", "machine learning", "data science" } },
            { "Web", new List<string> { "html", "css", "javascript", "asp.net", "react", "backend", "frontend", "node.js", "angular", "vue" } },
            { "Mobil", new List<string> { "maui", "android", "ios", "kotlin", "flutter", "swift", "react native" } },
            { "Siber Güvenlik", new List<string> { "security", "siber güvenlik", "network", "ağ", "linux", "cybersecurity", "penetration" } },
            { "DevOps", new List<string> { "docker", "github", "ci/cd", "cloud", "azure", "aws", "kubernetes", "jenkins" } }
        };

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SuitableStudentViewModel> CalculateMatchScoreAsync(CompanyProfile company, ApplicationUser student)
        {
            double totalScore = 0;
            var matchedFeatures = new List<string>();
            var missingSkills = new List<string>();

            // Student Corpus
            string studentCorpus = $"{student.ShortDescription} {student.Department} {student.University}".ToLowerInvariant();

            // 1. Base Score (Max 20%)
            // Give 20 points if profile has some basic info so no one is at 0-5%
            double baseScore = 0;
            if (!string.IsNullOrWhiteSpace(student.Department) || !string.IsNullOrWhiteSpace(student.City) || !string.IsNullOrWhiteSpace(student.ShortDescription))
            {
                baseScore = 20.0;
            }
            totalScore += baseScore;

            // 2. Department & Sector Match (Max 25%)
            double eduScore = 0;
            string targetArea = !string.IsNullOrWhiteSpace(company.RelatedArea) ? company.RelatedArea : company.Sector;
            if (!string.IsNullOrWhiteSpace(targetArea))
            {
                string targetLower = targetArea.ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(student.Department) && student.Department.ToLowerInvariant().Contains(targetLower))
                {
                    eduScore += 25.0;
                    matchedFeatures.Add("Bölüm/Alan Uyumu");
                }
                else if (!string.IsNullOrWhiteSpace(student.ShortDescription) && student.ShortDescription.ToLowerInvariant().Contains(targetLower))
                {
                    eduScore += 15.0;
                    matchedFeatures.Add("Alan İlgi/Deneyim");
                }
            }
            else
            {
                eduScore = 25.0; // If company didn't specify, don't penalize
            }
            totalScore += eduScore;

            // 3. Skills & Conceptual Match (Max 30%)
            double skillsScore = 0;
            if (!string.IsNullOrWhiteSpace(company.RequiredSkills))
            {
                var skills = company.RequiredSkills.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(s => s.Trim())
                                                   .Where(s => !string.IsNullOrEmpty(s))
                                                   .ToList();
                
                if (skills.Any())
                {
                    int matchedCount = 0;
                    foreach (var skill in skills)
                    {
                        bool matched = false;
                        string skillLower = skill.ToLowerInvariant();

                        // Exact Match
                        if (studentCorpus.Contains(skillLower))
                        {
                            matched = true;
                        }
                        else
                        {
                            // Concept Match
                            // Find concepts that contain this skill, or if the skill is a concept key
                            var relatedConcepts = new List<string>();
                            
                            // If the required skill matches a concept key
                            if (_conceptMap.Keys.Any(k => k.Equals(skill, StringComparison.OrdinalIgnoreCase) || k.ToLowerInvariant().Contains(skillLower)))
                            {
                                var key = _conceptMap.Keys.First(k => k.Equals(skill, StringComparison.OrdinalIgnoreCase) || k.ToLowerInvariant().Contains(skillLower));
                                relatedConcepts.AddRange(_conceptMap[key]);
                            }

                            // If the required skill is found inside a concept list, consider other words in that list
                            foreach (var kvp in _conceptMap)
                            {
                                if (kvp.Value.Any(v => v.Contains(skillLower)))
                                {
                                    relatedConcepts.Add(kvp.Key.ToLowerInvariant());
                                    relatedConcepts.AddRange(kvp.Value);
                                }
                            }

                            // Check if student corpus contains any related concepts
                            if (relatedConcepts.Any(rc => studentCorpus.Contains(rc)))
                            {
                                matched = true;
                            }
                        }

                        if (matched)
                        {
                            matchedCount++;
                            matchedFeatures.Add(skill);
                        }
                        else
                        {
                            missingSkills.Add(skill);
                        }
                    }
                    skillsScore = ((double)matchedCount / skills.Count) * 30.0;
                }
                else
                {
                    skillsScore = 30.0; 
                }
            }
            else
            {
                skillsScore = 30.0;
            }
            totalScore += skillsScore;

            // 4. PredictionHistory Match (Max 15%)
            double predictionScore = 0;
            if (!string.IsNullOrWhiteSpace(targetArea))
            {
                var pastPredictions = await _context.PredictionHistories
                    .Where(p => p.UserId == student.Id)
                    .Select(p => p.Prediction)
                    .ToListAsync();

                if (pastPredictions.Any(p => p != null && p.ToLowerInvariant().Contains(targetArea.ToLowerInvariant())))
                {
                    predictionScore = 15.0;
                    matchedFeatures.Add("Kariyer Tahmini Uyumu");
                }
            }
            else
            {
                predictionScore = 15.0;
            }
            totalScore += predictionScore;

            // 5. City Match (Max 5%)
            double cityScore = 0;
            if (!string.IsNullOrWhiteSpace(company.City) && !string.IsNullOrWhiteSpace(student.City))
            {
                if (company.City.Equals(student.City, StringComparison.OrdinalIgnoreCase))
                {
                    cityScore = 5.0;
                    matchedFeatures.Add("Aynı Şehir");
                }
            }
            else if (string.IsNullOrWhiteSpace(company.City))
            {
                cityScore = 5.0;
            }
            totalScore += cityScore;

            // 6. CV & Profile Presence (Max 5%)
            double profileScore = 0;
            if (!string.IsNullOrWhiteSpace(student.CVPath))
            {
                profileScore = 5.0;
                matchedFeatures.Add("Özgeçmiş");
            }
            totalScore += profileScore;

            // Normalize Total Score
            int finalScore = (int)Math.Round(totalScore);
            if (finalScore > 100) finalScore = 100;

            // AI Insight Generation (Mapped to new % brackets)
            string insight = "";
            if (finalScore >= 80)
            {
                insight = $"Profiliniz şirket beklentileriyle çok yüksek oranda (%{finalScore}) örtüşüyor.";
                if (missingSkills.Any())
                {
                    var skill = missingSkills.First();
                    skill = char.ToUpper(skill[0]) + skill.Substring(1);
                    insight += $" Ancak {skill} alanında pratiğinizi artırabilirsiniz.";
                }
                else
                {
                    insight += " Tüm gereksinimleri fazlasıyla karşılıyorsunuz.";
                }
            }
            else if (finalScore >= 60)
            {
                insight = $"Pozisyon için potansiyel vadediyorsunuz (%{finalScore} uyum). Temel gereksinimler karşılanıyor.";
                if (missingSkills.Count > 0)
                {
                    var skill = missingSkills.First();
                    skill = char.ToUpper(skill[0]) + skill.Substring(1);
                    insight += $" Ancak özellikle {skill} konusunda pratik yapabilirsiniz.";
                }
            }
            else if (finalScore >= 35)
            {
                insight = $"Kısmen uygunsunuz (%{finalScore} uyum). İlgili alanlara yakınsınız ancak teknik yetkinliklerinizi güçlendirmeniz gerekebilir.";
                if (missingSkills.Count > 1)
                {
                    var skill1 = missingSkills[0];
                    skill1 = char.ToUpper(skill1[0]) + skill1.Substring(1);
                    var skill2 = missingSkills[1];
                    skill2 = char.ToUpper(skill2[0]) + skill2.Substring(1);
                    insight += $" Özellikle {skill1} ve {skill2} öğrenimine odaklanın.";
                }
            }
            else
            {
                insight = "Mevcut yetkinlikleriniz bu rolün beklentileriyle tam örtüşmüyor, yetkinliklerinize daha uygun farklı kariyer alanlarını değerlendirebilirsiniz.";
            }

            // Cleanup matched features formatting
            var formattedMatchedFeatures = matchedFeatures
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(f => char.ToUpper(f[0]) + f.Substring(1))
                .Distinct()
                .ToList();
                
            var formattedMissingSkills = missingSkills
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(f => char.ToUpper(f[0]) + f.Substring(1))
                .Distinct()
                .ToList();

            return new SuitableStudentViewModel
            {
                Id = student.Id,
                FullName = string.IsNullOrWhiteSpace(student.FullName) ? (student.UserName ?? "Öğrenci") : student.FullName,
                University = student.University,
                Department = student.Department,
                City = student.City,
                ShortDescription = student.ShortDescription,
                CVPath = student.CVPath,
                Score = finalScore,
                MatchedFeatures = formattedMatchedFeatures,
                MissingSkills = formattedMissingSkills,
                Insight = insight
            };
        }
    }
}
