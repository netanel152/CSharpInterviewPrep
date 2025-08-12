namespace CSharpInterviewPrep.Exercises;

public static class B_ProblemSolving
{
    public static int? FindFirstDuplicate(int[] numbers)
    {
        var seenNumbers = new HashSet<int>();
        foreach (var number in numbers)
        {
            if (!seenNumbers.Add(number))
            {
                return number;
            }
        }
        return null;
    }

    public static bool AreAnagrams(string str1, string str2)
    {
        if (str1.Length != str2.Length) return false;

        var sortedStr1 = string.Concat(str1.OrderBy(c => c));
        var sortedStr2 = string.Concat(str2.OrderBy(c => c));
        return sortedStr1.Equals(sortedStr2);
    }

    public static List<string> GetTop10CommonWords(string text)
    {
        var skipwords = new HashSet<string> { "a", "an", "the", "is", "in", "it", "of" };
        var words = text
            .ToLower()
            .Split(new[] { ' ', '.', ',', ';', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !skipwords.Contains(word));

        var wordCounts = new Dictionary<string, int>();
        foreach (var word in words)
        {
            wordCounts.TryGetValue(word, out int currentCount);
            wordCounts[word] = currentCount + 1;
        }

        return wordCounts
            .OrderByDescending(kv => kv.Value)
            .Take(10).Select(kv => kv.Key)
            .ToList();
    }

    public static string RotateArray(int[] arr, int k)
    {
        if (arr == null || arr.Length == 0 || k < 0)
        {
            throw new ArgumentException("Invalid input array or rotation count.");
        }
        k = k % arr.Length; // Handle cases where k is larger than array length
        if (k == 0) return string.Join(", ", arr);
        int[] rotatedArray = new int[arr.Length];
        Array.Copy(arr, arr.Length - k, rotatedArray, 0, k);
        Array.Copy(arr, 0, rotatedArray, k, arr.Length - k);
        return string.Join(", ", rotatedArray);
    }
}