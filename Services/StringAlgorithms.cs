using System.Text;

namespace CSharpInterviewPrep.Services;

/// <summary>
/// Provides common algorithms and manipulations for strings.
/// </summary>
public static class StringAlgorithms
{
    /// <summary>
    /// Counts the occurrences of each letter in the text (case-insensitive).
    /// </summary>
    /// <param name="text">The input string.</param>
    /// <returns>A dictionary where keys are characters and values are their counts.</returns>
    public static Dictionary<char, int> CountCharacters(string text)
    {
        var counts = new Dictionary<char, int>();
        foreach (var c in text)
        {
            if (char.IsLetter(c))
            {
                var lChar = char.ToLower(c);
                if (counts.TryGetValue(lChar, out int value))
                {
                    counts[lChar] = ++value;
                }
                else
                {
                    counts[lChar] = 1;
                }
            }
        }
        return counts;
    }

    /// <summary>
    /// Finds the most frequent character in the input string.
    /// </summary>
    public static char GetMostFrequentChar(string input)
    {
        return input
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
    }

    /// <summary>
    /// Encodes a plain text string to Base64.
    /// </summary>
    public static string EncodeToBase64(string plainText)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decodes a Base64 encoded string back to plain text.
    /// </summary>
    public static string DecodeFromBase64(string base64String)
    {
        var bytes = Convert.FromBase64String(base64String);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Reverses the input string using Array.Reverse.
    /// </summary>
    public static string ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <summary>
    /// Checks if a string is a palindrome using string reversal.
    /// </summary>
    public static bool IsPalindrome(string input)
    {
        string reversed = ReverseString(input);
        return string.Equals(input, reversed, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if a string is a palindrome using the two-pointer technique (optimized).
    /// Ignores non-alphanumeric characters and case.
    /// </summary>
    public static bool IsPalindromeTwoPointer(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length == 1)
        {
            return true;
        }

        // Filter valid characters first to avoid index complexity
        // Note: For very large strings, a custom iterator would be more memory efficient
        // to avoid creating a new string.
        var filtered = input.Where(char.IsLetterOrDigit).ToArray();
        
        int left = 0;
        int right = filtered.Length - 1;

        while (left < right)
        {
            if (char.ToLower(filtered[left]) != char.ToLower(filtered[right])) 
                return false;
            left++;
            right--;
        }
        return true;
    }

    /// <summary>
    /// Checks if two strings are anagrams of each other.
    /// </summary>
    public static bool AreAnagrams(string str1, string str2)
    {
        if (str1.Length != str2.Length) return false;

        // Sorting is O(N log N). A frequency map (dictionary or array[26]) would be O(N).
        var sortedStr1 = string.Concat(str1.OrderBy(c => c));
        var sortedStr2 = string.Concat(str2.OrderBy(c => c));
        return sortedStr1.Equals(sortedStr2);
    }

    /// <summary>
    /// Returns the top 10 most common words in a text, ignoring common stop words.
    /// </summary>
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
            .Take(10)
            .Select(kv => kv.Key)
            .ToList();
    }
}