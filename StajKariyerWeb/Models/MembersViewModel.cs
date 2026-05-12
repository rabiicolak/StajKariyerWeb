using System.Collections.Generic;

namespace StajKariyerWeb.Models
{
    public class MembersViewModel
    {
        public List<SimilarMemberDto> TopSimilarMembers { get; set; } = new List<SimilarMemberDto>();
        public List<ApplicationUser> AllOtherMembers { get; set; } = new List<ApplicationUser>();
    }

    public class SimilarMemberDto
    {
        public ApplicationUser User { get; set; } = null!;
        public int SimilarityScore { get; set; }
        public List<string> MatchReasons { get; set; } = new List<string>();
    }
}
