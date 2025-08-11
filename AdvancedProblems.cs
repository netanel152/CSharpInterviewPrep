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
    }
}
