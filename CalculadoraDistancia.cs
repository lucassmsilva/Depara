namespace Depara;
using System.Text;

public class CalculadorDistancia
{
    public int Distance { get; private set; }
    public double Similiarity { get; private set; }
    public double Possibility { get; private set; }

    public string Base { get; set; }
    public string Comparacao { get; set; }

    public CalculadorDistancia(string str1, string str2, int threeshould = 2)
    {
        Base = str1;
        Comparacao = str2;
        Distance = LevenshteinDistance(str1, str2);
        Similiarity = JaccardSimilarity(str1, str2);

        var comparisonBetweenOneWord = (str1.Split(' ').Length == 1 && str2.Split(' ').Length == 1);

        if (comparisonBetweenOneWord)
        {
            if (Distance <= threeshould) // Aqui usar com um grão de sal, se o threeshould deve dar 1, 0, ou fazer uma razão;
                Possibility = (double)(str1.Length - Distance) / str1.Length;
            else
                Possibility = 0;
        }
        else
        {
            Possibility = JaccardLevenshteinCombined(str1, str2, threeshould);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("");
        sb.AppendLine(@"-----------------------------CALCULADORA----------------------------------");
        sb.AppendLine($"Distância de Levenshtein entre '{Base}' e '{Comparacao}' é: {Distance}");
        sb.AppendLine($"Similaridade de Jaccard: {Similiarity * 100}%");
        sb.AppendLine($"Possibilidade de Combinação: {Possibility * 100}%");
        sb.AppendLine(@"--------------------------------------------------------------------------");
        sb.AppendLine("");


        return sb.ToString();
    }

    public static double JaccardSimilarity(string s1, string s2)
    {
        var set1 = new HashSet<string>(s1.Split(' '));
        var set2 = new HashSet<string>(s2.Split(' '));

        var intersection = new HashSet<string>(set1);
        intersection.IntersectWith(set2);

        var union = new HashSet<string>(set1);
        union.UnionWith(set2);

        return (double)intersection.Count / union.Count;
    }

    public static int LevenshteinDistance(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1))
            return string.IsNullOrEmpty(s2) ? 0 : s2.Length;

        if (string.IsNullOrEmpty(s2))
            return s1.Length;

        int[,] distances = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; distances[i, 0] = i++)
        {
        }

        for (int j = 0; j <= s2.Length; distances[0, j] = j++)
        {
        }

        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                int cost = (s2[j - 1] == s1[i - 1]) ? 0 : 1;

                distances[i, j] = Math.Min(
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost);
            }
        }

        return distances[s1.Length, s2.Length];
    }


    public static double JaccardLevenshteinCombined(string s1, string s2, int levenshteinThreshold)
    {
        // Create sets of words
        var set1 = new HashSet<string>(s1.ToLower().Split(' '));
        var set2 = new HashSet<string>(s2.ToLower().Split(' '));

        var union = new HashSet<string>(set1);
        union.UnionWith(set2);

        var intersection = new HashSet<string>(set1);
        intersection.IntersectWith(set2);

        var difference = new HashSet<string>(union);
        difference.ExceptWith(intersection);

        var wordsToRemove = new HashSet<string>();

        foreach (var word1 in difference)
        {
            foreach (var word2 in difference)
            {
                if (word1 != word2 && !wordsToRemove.Contains(word1) && !wordsToRemove.Contains(word2))
                {
                    int distance = LevenshteinDistance(word1, word2);
                    if (distance <= levenshteinThreshold)
                    {
                        wordsToRemove.Add(word1);
                        wordsToRemove.Add(word2);
                        break;
                    }
                }
            }
        }

        difference.ExceptWith(wordsToRemove);

        double jaccardSimilarity = (double)intersection.Count / union.Count;
        double levenshteinFactor = 1 - ((double)difference.Count / union.Count);

        return (jaccardSimilarity + levenshteinFactor) / 2;
    }
}