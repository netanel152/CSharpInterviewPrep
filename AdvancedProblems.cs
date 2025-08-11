using System.Diagnostics;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep
{
    public class AdvancedProblems
    {
        // מסנן הרשאות
        public static List<SearchResult> FilterResultsByPermissions(List<SearchResult> results, User user)
        {
            var userGroups = new HashSet<string>(user.MemberOfGroups);

            return results.Where(result =>
                result.AllowedGroups.Any(group => userGroups.Contains(group))
            ).ToList();
        }

        // ממזג מסמכים
        public static Document MergeDocuments(Document oldVersion, Document newVersion)
        {
            var mergedFields = new Dictionary<string, string>(oldVersion.Fields);

            foreach (var field in newVersion.Fields)
            {
                mergedFields[field.Key] = field.Value;
            }

            return new Document { Fields = mergedFields };
        }

        // מחלקת השלמה אוטומטית
        public class AutocompleteService
        {
            private readonly List<string> _popularSearches;

            public AutocompleteService(List<string> popularSearches)
            {
                _popularSearches = popularSearches;
            }

            public List<string> GetSuggestions(string prefix)
            {
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    return new List<string>();
                }

                return _popularSearches
                    .Where(s => s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    .Take(5)
                    .ToList();
            }
        }

        public class AsyncDataProcessor
        {
            // Simulates a slow API call
            private async Task<string> GetUserNameAsync(int userId)
            {
                await Task.Delay(1000); // Wait for 1 second
                return "John Doe";
            }

            // Simulates a very slow database query
            private async Task<List<string>> GetUserPermissionsAsync(int userId)
            {
                await Task.Delay(2000); // Wait for 2 seconds
                return new List<string> { "read", "write" };
            }

            // Simulates a fast API call
            private async Task<List<string>> GetUserRecentActivityAsync(int userId)
            {
                await Task.Delay(500); // Wait for 500 ms
                return new List<string> { "Logged in", "Viewed page" };
            }

            public async Task<UserProfile> ProcessUserDataAsync(int userId)
            {
                var stopwatch = Stopwatch.StartNew();

                var nameTask = GetUserNameAsync(userId);
                var permissionsTask = GetUserPermissionsAsync(userId);
                var activityTask = GetUserRecentActivityAsync(userId);

                // חכה שכל המשימות שהתנענו יסתיימו
                await Task.WhenAll(nameTask, permissionsTask, activityTask);

                var userProfile = new UserProfile
                {
                    Name = await nameTask,
                    Permissions = await permissionsTask,
                    RecentActivity = await activityTask
                };

                stopwatch.Stop();
                Console.WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine(
                    $"User Profile: {userProfile.Name},\n" +
                    $"Permissions: {string.Join(", ", userProfile.Permissions)},\n" +
                    $"Recent Activity: {string.Join(", ", userProfile.RecentActivity)}");
                return userProfile;
            }
        }
    }
}
