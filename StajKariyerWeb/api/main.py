from fastapi import FastAPI
import joblib
import pandas as pd

app = FastAPI()

# Model ve yardımcı dosyaları yükle
model = joblib.load("model.pkl")
columns = joblib.load("model_columns.pkl")
company_df = pd.read_excel("firmaVeri.xlsx")

# Firma verisini temizle
company_df["Ilgili_Alan"] = company_df["Ilgili_Alan"].astype(str).str.strip()
company_df["FİRMA"] = company_df["FİRMA"].astype(str).str.strip()
company_df["GNO"] = pd.to_numeric(company_df["GNO"], errors="coerce").fillna(0)

# Teknik beceri sütunlarını da güvenli hale getir
for col in ["Python", "Java", "Csharp", "C++", "Veritabani"]:
    company_df[col] = pd.to_numeric(company_df[col], errors="coerce").fillna(0)


def map_predicted_area(predicted):
    mapping = {
        "Yapay Zeka Araştırmacısı": "Yapay Zeka",
        "Yapay Zeka Mühendisi": "Yapay Zeka",
        "Veri Bilimci": "Veri Bilimi",
        "Web Geliştirici": "Web",
        "Mobil Uygulama Geliştirici": "Mobil",
        "Bilgi Güvenliği Analisti": "Siber Güvenlik",
        "Bulut Çözümleri Mimarı": "DevOps",
        "Yazılım Mühendisi": "Backend",
        "Veritabanı Yöneticisi": "Backend",
        "Grafik Programcısı": "Oyun Geliştirme",
        "Oyun Geliştirici": "Oyun Geliştirme",
        "Sağlık Bilişimi Uzmanı": "Veri Bilimi"
    }
    return mapping.get(predicted, predicted)


def calculate_match_score(student_profile, company_row):
    score = 0

    # Alan eşleşmesi
    if student_profile["alan"] == company_row["Ilgili_Alan"]:
        score += 40

    # Teknik beceriler
    if student_profile["Python"] >= company_row["Python"]:
        score += 10

    if student_profile["Java"] >= company_row["Java"]:
        score += 10

    if student_profile["Csharp"] >= company_row["Csharp"]:
        score += 10

    if student_profile["C++"] >= company_row["C++"]:
        score += 10

    if student_profile["Veritabani"] >= company_row["Veritabani"]:
        score += 10

    # GNO karşılaştırması
    if student_profile["GNO"] >= company_row["GNO"]:
        score += 10

    return score


def score_to_label(score):
    if score >= 90:
        return "Çok Uygun"
    elif score >= 70:
        return "Uygun"
    elif score >= 50:
        return "Kısmen Uygun"
    else:
        return "Uygun Değil"


@app.get("/")
def home():
    return {"message": "API çalışıyor"}


@app.get("/version")
def version():
    return {"version": "v5_decimal_gno_filtreli_api"}


@app.post("/predict")
def predict(data: dict):
    # Gelen veriyi DataFrame'e çevir
    df = pd.DataFrame([data])

    # Kategorik alanları modele uygun hale getir
    df_encoded = pd.get_dummies(df)
    df_encoded = df_encoded.reindex(columns=columns, fill_value=0)

    # Kariyer alanı tahmini
    prediction = model.predict(df_encoded)[0]
    matched_area = map_predicted_area(prediction)

    # Öğrenci profilini eşleştirme için hazırla
    student_profile = {
        "GNO": float(data["GNO"]),
        "Python": int(data["Python"]),
        "Java": int(data["Java"]),
        "Csharp": int(data["Csharp"]),
        "C++": int(data["C++"]),
        "Veritabani": int(data["Veritabani"]),
        "alan": matched_area
    }

    results = []

    # Tüm firmalar için skor hesapla
    for _, company in company_df.iterrows():
        score = calculate_match_score(student_profile, company)

        results.append({
            "FİRMA": company["FİRMA"],
            "Ilgili_Alan": company["Ilgili_Alan"],
            "Score": int(score),
            "Durum": score_to_label(score)
        })

    result_df = pd.DataFrame(results)

    # Sadece önerilebilir firmaları bırak
    result_df = result_df[result_df["Score"] >= 50]

    # En yüksek puanlı 5 firmayı getir
    result_df = result_df.sort_values(by="Score", ascending=False).head(5)

    # Hiç uygun firma yoksa mesaj dön
    if result_df.empty:
        return {
            "prediction": prediction,
            "matched_area": matched_area,
            "message": "Bu profile uygun firma bulunamadı.",
            "companies": []
        }

    # Sonucu döndür
    return {
        "prediction": prediction,
        "matched_area": matched_area,
        "message": None,
        "companies": result_df.to_dict(orient="records")
    }