using System.Collections.Concurrent;
using System.Reflection;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

public class E_SystemDesignProblems
{
    public class Leaderboard
    {
        private readonly Dictionary<int, int> _scores = new();
        public void UpdateScore(int playerId, int score)
        {
            if (_scores.ContainsKey(playerId))
            {
                _scores[playerId] += score;
            }
            else
            {
                _scores[playerId] = score;
            }
        }

        public List<int> GetTopPlayers(int count)
        {
            return _scores
                .OrderByDescending(kv => kv.Value)
                .Take(count)
                .Select(kv => kv.Key)
                .ToList();
        }
    }


    public class BadHttpService
    {
        // ❌ טעות: יצירת HttpClient חדש לכל קריאה גורמת לדליפת חיבורים (Socket Exhaustion)
        public async Task<string> FetchDataAsync(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }
    }

    public class GoodHttpService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        // ✅ תיקון: שימוש ב-HttpClient סטטי כדי למנוע דליפת חיבורים
        public async Task<string> FetchDataAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }
    }

    // נניח ש-users הוא IQueryable<User> שמגיע מ-Entity Framework
    public void ProcessAdmins_Bad(IQueryable<User> users)
    {
        var admins = users.Where(u => u.Role == "Admin");

        if (admins.Any()) //  ⬅️ שאילתה ראשונה ל-DB
        {
            Console.WriteLine($"Found {admins.Count()} admins."); // ⬅️ שאילתה שנייה ל-DB

            foreach (var admin in admins) // ⬅️ שאילתה שלישית ל-DB
            {
                Console.WriteLine($"Processing admin: {admin.Name}");
            }
        }
    }

    public void ProcessAdmins_Good(IQueryable<User> users)
    {
        List<User> admins = users.Where(u => u.Role == "Admin").ToList(); // ⬅️ שאילתה אחת ל-DB
        if (admins.Any())
        {
            Console.WriteLine($"Found {admins.Count} admins.");
            foreach (var admin in admins)
            {
                Console.WriteLine($"Processing admin: {admin.Name}");
            }
        }
    }

    public class LargeFileSorter
    {
        public void Sort(string largeFilePath, string sortedFilePath)
        {
            const int chunkSize = 100000; // מספר שורות שכן נכנס לזיכרון
            var tempFiles = new List<string>();

            // 1. שלב הפיצול (Split)
            using (var reader = new StreamReader(largeFilePath))
            {
                int fileCounter = 0;
                while (!reader.EndOfStream)
                {
                    var chunk = new List<string>(chunkSize);
                    for (int i = 0; i < chunkSize && !reader.EndOfStream; i++)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            chunk.Add(line);
                        }
                    }

                    chunk.Sort(); // מיין את החלק בזיכרון

                    string tempFile = $"temp_{fileCounter++}.txt";
                    File.WriteAllLines(tempFile, chunk);
                    tempFiles.Add(tempFile);
                }
            }

            // 2. שלב המיזוג (Merge) - זהו מימוש פשוט. מימוש מלא יהיה מורכב יותר.
            var allLines = new List<string>();
            foreach (var tempFile in tempFiles)
            {
                allLines.AddRange(File.ReadAllLines(tempFile));
                File.Delete(tempFile);
            }

            allLines.Sort();
            File.WriteAllLines(sortedFilePath, allLines);

            Console.WriteLine("Large file sorted successfully.");
        }
    }

    public class ChangeDetector
    {

        public static List<Change> FindChanges<T>(T oldState, T newState)
        {
            var changes = new List<Change>();
            if (oldState == null || newState == null) return changes;

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                var oldValue = prop.GetValue(oldState);
                var newValue = prop.GetValue(newState);

                if (!Equals(oldValue, newValue))
                {
                    changes.Add(new Change
                    {
                        PropertyName = prop.Name,
                        OldValue = oldValue?.ToString(),
                        NewValue = newValue?.ToString()
                    });
                }
            }
            return changes;
        }
    }


    public class ThreadSafeCounter
    {
        private int _value;
        public int Value => _value;
        public int Increment() => Interlocked.Increment(ref _value);
    }

    // 2. השירות המרכזי שמנהל את כל המונים
    public class LikeCounterService
    {
        private readonly ConcurrentDictionary<int, ThreadSafeCounter> _likeCounters = new();

        /// <summary>
        /// מקבל 'לייק' לשיר ומגדיל את המונה שלו בצורה בטוחה.
        /// </summary>
        public void Like(int songId)
        {
            // GetOrAdd היא פעולה אטומית: היא או מחזירה את המונה הקיים,
            // או יוצרת חדש ומוסיפה אותו למילון, ומבטיחה שזה קורה רק פעם אחת.
            ThreadSafeCounter counter = _likeCounters.GetOrAdd(songId, _ => new ThreadSafeCounter());

            counter.Increment();
        }

        /// <summary>
        /// מחזיר את מספר הלייקים הנוכחי עבור שיר מסוים.
        /// </summary>
        public int GetLikeCount(int songId)
        {
            // TryGetValue היא הדרך היעילה לקרוא מ-ConcurrentDictionary.
            if (_likeCounters.TryGetValue(songId, out var counter))
            {
                return counter.Value;
            }
            return 0;
        }
    }

    public static void Run()
    {
        Console.WriteLine("--- Section 5: System Design Problems ---");
        //demonstration of the Lederboard system
        Console.WriteLine("--- Demonstrating Leaderboard System ---");
        var leaderboard = new E_SystemDesignProblems.Leaderboard();
        leaderboard.UpdateScore(1, 100);
        leaderboard.UpdateScore(2, 200);

        var topPlayers = leaderboard.GetTopPlayers(1);
        Console.WriteLine($"Top player ID: {topPlayers.FirstOrDefault()}");
        //demonstration of the BadHttpService and GoodHttpService
        //Console.WriteLine("--- Demonstrating Bad and Good HTTP Services ---");
        //var badService = new BadHttpService();
        //try
        //{
        //    string badData = await badService.FetchDataAsync("http://example.com");
        //    Console.WriteLine($"Bad HTTP Service fetched data: {badData.Substring(0, 50)}...");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Bad HTTP Service error: {ex.Message}");
        //}
        //var goodService = new GoodHttpService();
        //try
        //{
        //    string goodData = await goodService.FetchDataAsync("http://example.com");
        //    Console.WriteLine($"Good HTTP Service fetched data: {goodData.Substring(0, 50)}...");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Good HTTP Service error: {ex.Message}");
        //}

        //demonstration of the LargeFileSorter
        Console.WriteLine("--- Demonstrating Large File Sorter ---");
        //var largeFileSorter = new LargeFileSorter();
        //var unsortedFilePath = "unsorted_numbers.txt"; // Assume this file exists with unsorted numbers
        //var sortedFilePath = "sorted_numbers.txt";
        //largeFileSorter.Sort(unsortedFilePath, sortedFilePath);
        //Console.WriteLine($"Large file sorted and saved to {sortedFilePath}");


        //demonstration of the ChangeDetector
        Console.WriteLine("--- Demonstrating Change Detector ---");

        //var fruitV1 = new Fruit
        //{
        //    Name = "Apple",
        //    Color = "Red",
        //    Price = 1.2m
        //};

        //var fruitV2 = new Fruit
        //{
        //    Name = "Apple",
        //    Color = "Green",
        //    Price = 1.3m
        //};

        //var changes = ChangeDetector.FindChanges(fruitV1, fruitV2);

        //Console.WriteLine("Changes detected between fruitV1 and fruitV2:");
        //if (changes.Any())
        //{
        //    foreach (var change in changes)
        //    {
        //        Console.WriteLine($"- Property '{change.PropertyName}' changed from '{change.OldValue}' to '{change.NewValue}'");
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("No changes found.");
        //}


        Console.WriteLine("\n--- System Design: Thread-Safe Like Counter Demo ---");
        var likeService = new LikeCounterService();
        const int songId = 123;
        const int likesPerThread = 100_000;
        const int numberOfThreads = 10;
        const int expectedLikes = likesPerThread * numberOfThreads;

        var threads = new List<Thread>();

        for (int i = 0; i < numberOfThreads; i++)
        {
            var thread = new Thread(() =>
            {
                for (int j = 0; j < likesPerThread; j++)
                {
                    likeService.Like(songId);
                }
            });
            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join(); // המתן לסיום כל ה-Threads
        }

        int finalCount = likeService.GetLikeCount(songId);
        Console.WriteLine($"Final like count for song {songId}: {finalCount:N0}");
        Console.WriteLine($"Expected like count: {expectedLikes:N0}");
        Console.WriteLine($"Success: {finalCount == expectedLikes}");
        Console.WriteLine("---------------------------------\n");
    }
}
