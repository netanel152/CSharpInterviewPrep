using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep
{
    public static class CoreConcepts
    {
        public static Dictionary<char, int> CountCharcters(string input)
        {
            var characterCount = new Dictionary<char, int>();
            foreach (char character in input)
            {
                if (char.IsWhiteSpace(character))
                {
                    continue;
                }
                else if (characterCount.ContainsKey(character))
                {
                    characterCount[character]++;
                }
                else
                {
                    characterCount[character] = 1;
                }
            }
            return characterCount;
        }

        public static Dictionary<int, string> CreateProductLookup(List<Product> products)
        {
            return products.ToDictionary(p => p.Id, p => p.Name);
        }

        public static List<int> returnOnlyEventNumbers(List<int> numbers)
        {
            return numbers.Where(n => n % 2 == 0).Select(n => n * 2).ToList();
        }

        public static Dictionary<string, decimal> GroupOrdersByCategory(List<Order> orders)
        {
            return orders.GroupBy(o => o.Category)
                         .ToDictionary(g => g.Key, g => g.Sum(o => o.Price));
        }

        public static List<Student> SortStudentsByGrade(List<Student> students)
        {
            return students.OrderByDescending(s => s.Grade).Take(3).ToList();
        }

    }
}
