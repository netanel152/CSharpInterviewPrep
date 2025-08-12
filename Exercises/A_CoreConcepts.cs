using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

public static class A_CoreConcepts
{
    public static Dictionary<char, int> CountCharacters(string text)
    {
        var counts = new Dictionary<char, int>();
        foreach (char c in text.Where(c => !char.IsWhiteSpace(c)))
        {
            counts.TryGetValue(c, out int currentCount);
            counts[c] = currentCount + 1;
        }
        return counts;
    }

    public static Dictionary<int, string> CreateProductLookup(List<Product> products)
    {
        return products.ToDictionary(p => p.Id, p => p.Name);
    }

    public static List<int> FilterAndDoubleEvens(List<int> numbers)
    {
        return numbers.Where(n => n % 2 == 0).Select(n => n * 2).ToList();
    }

    public static Dictionary<string, decimal> GetSalesByCategory(List<Order> orders)
    {
        return orders
            .GroupBy(o => o.Category)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.Price));
    }

    public static List<Student> GetTopStudents(List<Student> students)
    {
        return students.OrderByDescending(s => s.Grade).Take(3).ToList();
    }
}