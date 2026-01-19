using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Services;

/// <summary>
/// Provides methods for processing collections of data using LINQ and other techniques.
/// </summary>
public static class DataProcessingService
{
    public static Dictionary<int, string> CreateProductLookup(List<Product> products)
    {
        return products.ToDictionary(p => p.Id, p => p.Name);
    }

    public static List<int> FilterAndDoubleEvens(List<int> numbers)
    {
        return numbers
            .Where(n => n % 2 == 0)
            .Select(n => n * 2)
            .ToList();
    }

    public static Dictionary<string, decimal> GetSalesByCategory(List<Order> orders)
    {
        return orders
            .GroupBy(o => o.Category)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.Price));
    }

    public static List<Student> GetTopStudents(List<Student> students, int count = 3)
    {
        return students
            .OrderByDescending(s => s.Grade)
            .Take(count)
            .ToList();
    }
}