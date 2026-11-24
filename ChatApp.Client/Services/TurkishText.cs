namespace ChatApp.Client.Services;

// İsimlere Türkçe ses uyumuna göre ek getirir: "Ali'ye", "Mehmet'e", "Ayşe'ye", "Can'a"
public static class TurkishText
{
    private const string BackVowels = "aıouAIOU";
    private const string Vowels = "aeıioöuüAEIİOÖUÜ";

    // Yönelme (-e / -a) hâli eki
    public static string Dative(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        // Son ünlü kalınsa (a, ı, o, u) "-a", inceyse "-e"; ünlü yoksa "-e" kabul edilir
        int index = name.LastIndexOfAny(Vowels.ToCharArray());
        bool back = index >= 0 && BackVowels.Contains(name[index]);
        bool endsWithVowel = Vowels.Contains(name[^1]);

        string suffix = (endsWithVowel ? "y" : "") + (back ? "a" : "e");
        return $"{name}'{suffix}";
    }
}
