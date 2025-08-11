using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep
{
    public static class ProblemSolving
    {
        public static int? FindFirstDuplicate(int[] numbers)
        {
            HashSet<int> seenNumbers = [];
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
            if (str1.Length != str2.Length)
            {
                return false;
            }
            string sortedStr1 = string.Concat(str1.OrderBy(c => c));
            string sortedStr2 = string.Concat(str2.OrderBy(c => c));
            var isAnagram = sortedStr1.Equals(sortedStr2);
            return isAnagram;
        }

        public static List<string> GetTop10CommonWords(string text)
        {
            var skipwords = new HashSet<string> { "a", "an", "the", "the", "is", "in", "it", "of" };

            var words = text.ToLower()
                            .Split([' ', '.', ',', ';', ':', '?'], StringSplitOptions.RemoveEmptyEntries)
                            .Where(word => !skipwords.Contains(word));

            var wordCounts = new Dictionary<string, int>();
            foreach (var word in words)
            {
                wordCounts.TryGetValue(word, out int currentCount);
                wordCounts[word] = currentCount + 1;
            }

            var top10Words = wordCounts.OrderByDescending(pair => pair.Value)
                             .Take(10)
                             .Select(pair => pair.Key)
                             .ToList();

            return top10Words;
        }

        public static List<List<string>> FindDuplicateFiles(List<FileMetadata> files)
        {
            var filesByHash = new Dictionary<string, List<string>>();

            foreach (var file in files)
            {
                if (!filesByHash.ContainsKey(file.FileHash))
                {
                    filesByHash[file.FileHash] = new List<string>();
                }
                filesByHash[file.FileHash].Add(file.FilePath);
            }

            var duplicateFileGroups = filesByHash.Values
                              .Where(group => group.Count > 1)
                              .ToList();

            return duplicateFileGroups;
        }

        public static List<string> GetAllFilesInDirectory(string rootPath)
        {
            var allFiles = new List<string>();

            try
            {
                allFiles.AddRange(Directory.GetFiles(rootPath));

                var subDirectories = Directory.GetDirectories(rootPath);
                foreach (var dir in subDirectories)
                {
                    allFiles.AddRange(GetAllFilesInDirectory(dir));
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied to: {rootPath}. Skipping. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred at {rootPath}. Error: {ex.Message}");
            }

            return allFiles;
        }

        public static List<Document> RankSearchResults(string query, List<Document> documents)
        {
            var rankedDocs = documents
                .Select(doc => new
                {
                    Document = doc,
                    Score = System.Text.RegularExpressions.Regex.Matches(doc.Content.ToLower(), query.ToLower()).Count
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Document.CreatedAt)
                .Select(x => x.Document)
                .ToList();

            return rankedDocs;
        }

        public static (int Min, int Max) FindMinMax(int[] numbers)
        {
            int minVal = int.MaxValue;
            int maxVal = int.MinValue;

            foreach (var num in numbers)
            {
                if (num < minVal) minVal = num;
                if (num > maxVal) maxVal = num;
            }

            return (minVal, maxVal);
        }
    }

    public class Document
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new();
    }

    public class ShoppingCart
    {
        private readonly List<Item> _items = [];

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public decimal GetTotalPrice()
        {
            return _items.Sum(item => item.Price);
        }
    }
}
