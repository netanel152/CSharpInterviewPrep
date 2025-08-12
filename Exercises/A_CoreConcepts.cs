using System.Text;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

public static class A_CoreConcepts
{
    public static Dictionary<char, int> CountCharacters(string text)
    {
        var counts = new Dictionary<char, int>();
        foreach (var c in text)
        {
            if (char.IsLetter(c))
            {
                var lowerChar = char.ToLower(c);
                if (counts.TryGetValue(lowerChar, out int value))
                {
                    counts[lowerChar] = ++value;
                }
                else
                {
                    counts[lowerChar] = 1;
                }
            }
        }
        return counts;
    }

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
            .ToDictionary(g => g.Key, g => g
            .Sum(o => o.Price));
    }

    public static List<Student> GetTopStudents(List<Student> students)
    {
        return students
            .OrderByDescending(s => s.Grade)
            .Take(3)
            .ToList();
    }

    public static string EncodeToBase64(string plainText)
    {
        var bayts = Encoding.UTF8.GetBytes(plainText);
        var base64String = Convert.ToBase64String(bayts);
        return base64String;
    }

    public static string DecodeFromBase64(string base64String)
    {
        var bayts = Convert.FromBase64String(base64String);
        var plainText = Encoding.UTF8.GetString(bayts);
        return plainText;
    }

    //create more core concepts methods as needed

    public static void PrintHelloWorld()
    {
        Console.WriteLine("Hello, World!");
    }

    public static void PrintCurrentDateTime()
    {
        Console.WriteLine($"Current DateTime: {DateTime.Now}");
    }

    public static void PrintRandomNumber()
    {
        var random = new Random();
        Console.WriteLine($"Random Number: {random.Next(1, 100)}");
    }

    public static void PrintSumOfTwoNumbers(int a, int b)
    {
        Console.WriteLine($"Sum of {a} and {b} is {a + b}");
    }

    public static void PrintProductOfTwoNumbers(int a, int b)
    {
        Console.WriteLine($"Product of {a} and {b} is {a * b}");
    }

    public static void PrintGreeting(string name)
    {
        Console.WriteLine($"Hello, {name}!");
    }

    public static void PrintEvenNumbers(int limit)
    {
        Console.WriteLine($"Even numbers up to {limit}:");
        for (int i = 0; i <= limit; i += 2)
        {
            Console.WriteLine(i);
        }
    }
    public static void PrintOddNumbers(int limit)
    {
        Console.WriteLine($"Odd numbers up to {limit}:");
        for (int i = 1; i <= limit; i += 2)
        {
            Console.WriteLine(i);
        }
    }
    public static void PrintMultiplicationTable(int number, int limit = 10)
    {
        Console.WriteLine($"Multiplication table for {number}:");
        for (int i = 1; i <= limit; i++)
        {
            Console.WriteLine($"{number} x {i} = {number * i}");
        }
    }

    public static void PrintFibonacciSeries(int terms)
    {
        Console.WriteLine($"Fibonacci series up to {terms} terms:");
        int a = 0, b = 1;
        for (int i = 0; i < terms; i++)
        {
            Console.WriteLine(a);
            int next = a + b;
            a = b;
            b = next;
        }
    }

    public static void ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        string reversed = new string(charArray);
        Console.WriteLine($"Reversed string: {reversed}");
    }

    public static void PrintArrayElements(int[] array)
    {
        Console.WriteLine("Array elements:");
        foreach (var item in array)
        {
            Console.WriteLine(item);
        }
    }

    public static void PrintListElements(List<string> list)
    {
        Console.WriteLine("List elements:");
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
    }

    public static void PrintDictionaryElements(Dictionary<string, int> dictionary)
    {
        Console.WriteLine("Dictionary elements:");
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

}