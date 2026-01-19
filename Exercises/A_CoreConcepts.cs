using CSharpInterviewPrep.Services;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

public static class A_CoreConcepts
{
    public static void Run()
    {
        Console.WriteLine("--- Section 1: Core C# Concepts ---");

        Console.WriteLine("--- Demonstrating CountCharacters ---");
        var charCounts = StringAlgorithms.CountCharacters("hello world");
        Console.WriteLine($"CountCharacters('hello world'): {charCounts.Count} distinct chars found.");

        string input = "abracadabracdb";
        char mostFrequentChar = StringAlgorithms.GetMostFrequentChar(input);
        Console.WriteLine($"GetMostFrequentChar('{input}'): Most frequent char is '{mostFrequentChar}'.");

        var products = new List<Product> { new() { Id = 1, Name = "Laptop" }, new() { Id = 2, Name = "Smartphone" } };
        var productLookup = DataProcessingService.CreateProductLookup(products);
        Console.WriteLine($"CreateProductLookup: Found {productLookup.Count} products.");

        var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        var doubledEvens = DataProcessingService.FilterAndDoubleEvens(numbers);
        Console.WriteLine($"FilterAndDoubleEvens: Found {doubledEvens.Count} even numbers, doubled them to {string.Join(", ", doubledEvens)}.");
        
        var orders = new List<Order>
        {
            new() { Category = "Electronics", Price = 100 },
            new() { Category = "Books", Price = 50 },
            new() { Category = "Electronics", Price = 200 }
        };
        var salesByCategory = DataProcessingService.GetSalesByCategory(orders);
        Console.WriteLine($"GetSalesByCategory: Found {salesByCategory.Count} categories with total sales.");
        foreach (var category in salesByCategory)
        {
            Console.WriteLine($"Category: {category.Key}, Total Sales: {category.Value:C}");
        }
        
        var students = new List<Student> { new() { Name = "Alice", Grade = 95 }, new() { Name = "Bob", Grade = 80 }, new() { Name = "Charlie", Grade = 92 } };
        var topStudents = DataProcessingService.GetTopStudents(students);
        Console.WriteLine($"GetTopStudents: Top student is {topStudents.First().Name}.");

        var base64Encoded = StringAlgorithms.EncodeToBase64("Hello World");
        Console.WriteLine($"EncodeToBase64: Encoded string is {base64Encoded}.");
        var decodedText = StringAlgorithms.DecodeFromBase64(base64Encoded);
        Console.WriteLine($"DecodeFromBase64: Decoded string is {decodedText}.");

        Console.WriteLine("--- Demonstrating PrintFibonacciSeries ---");

        foreach(var num in NumericAlgorithms.GenerateFibonacciSeries(10))
        {
            Console.WriteLine(num);
        }

        Console.WriteLine("--- Demonstrating ReverseString ---");
        string original = "Hello World";
        string reversed = StringAlgorithms.ReverseString(original);
        Console.WriteLine($"Original: {original}, Reversed: {reversed}");

        Console.WriteLine("--- Demonstrating IsPalindrome ---");
        string palindromeString = "madam";
        bool isPalindrome = StringAlgorithms.IsPalindrome(palindromeString);
        Console.WriteLine($"Is '{palindromeString}' a palindrome? {isPalindrome}");

        Console.WriteLine("--- Demonstrating MergeSortedArrays ---");
        int[] array1 = { 1, 3, 5 };
        int[] array2 = { 2, 4, 6, 7 };
        int[] mergedArray = ArrayAlgorithms.MergeSortedArrays(array1, array2);
        Console.WriteLine($"Merged array: {string.Join(", ", mergedArray)}");
        Console.WriteLine("---------------------------------\n");
    }
}