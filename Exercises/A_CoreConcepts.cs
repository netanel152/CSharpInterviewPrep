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


    public static char GetMostFrequentChar(string input)
    {
        return input
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
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

    public static void PrintCurrentDateTime()
    {
        Console.WriteLine($"Current DateTime: {DateTime.Now}");
    }

    public static void PrintRandomNumber()
    {
        var random = new Random();
        Console.WriteLine($"Random Number: {random.Next(1, 100)}");
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

    public static string ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        string reversed = new(charArray);
        return reversed;
    }

    internal static bool IsPalindrome(string palindrome)
    {
        string reversed = ReverseString(palindrome);
        return string.Equals(palindrome, reversed, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsPalindromeTwoPointer(string input)
    {
        // Empty string is a palindrome or single character is a palindrome
        if (string.IsNullOrEmpty(input) || input.Length == 1)
        {
            return true;
        } 

        input = new string(input.Where(c => char.IsLetterOrDigit(c)).ToArray());
        input = input.ToLower();
        int left = 0, right = input.Length - 1;
        while (left < right)
        {
            if (input[left] != input[right]) return false;
            left++;
            right--;
        }
        return true;
    }

    public static void FizzBuzz()
    {
        Enumerable.Range(1, 100)
            .ToList()
            .ForEach(n =>
            {
                if (n % 15 == 0) Console.WriteLine("FizzBuzz");
                else if (n % 3 == 0) Console.WriteLine("Fizz");
                else if (n % 5 == 0) Console.WriteLine("Buzz");
                else Console.WriteLine(n);
            });
    }

    public static int[] MergeSortedArrays(int[] arr1, int[] arr2)
    {
        int[] mergeArr = new int[arr1.Length + arr2.Length];
        int i = 0, j = 0, m = 0;

        while (i < arr1.Length && j < arr2.Length)
        {
            if (arr1[i] < arr2[j])
            {
                mergeArr[m++] = arr1[i++];
            }
            else
            {
                mergeArr[m++] = arr2[j++];
            }
        }

        while (i < arr1.Length)
        {
            mergeArr[m++] = arr1[i++];
        }
        while (j < arr2.Length)
        {
            mergeArr[m++] = arr2[j++];
        }

        return mergeArr;
    }



}