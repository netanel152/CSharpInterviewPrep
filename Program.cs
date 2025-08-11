namespace CSharpInterviewPrep;

public static class Program
{
    public static void Main(string[] args)
    {
        var characterCount = Day1.DataStructuresAndLinq.CountCharcters("hello world");
        foreach (var kvp in characterCount)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        Console.WriteLine("Character count completed. You can now proceed to the next operation.");

        var products = new List<Day1.Product>
    {
        new() { Id = 1, Name = "Product A" },
        new() { Id = 2, Name = "Product B" }
    };
        var productLookup = Day1.DataStructuresAndLinq.CreateProductLookup(products);
        foreach (var kvp in productLookup)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        Console.WriteLine("Product lookup completed. You can now proceed to the next operation.");

        var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        var evenNumbers = Day1.DataStructuresAndLinq.returnOnlyEventNumbers(numbers);
        Console.WriteLine("Even numbers multiplied by 2:");
        foreach (var number in evenNumbers)
        {
            Console.WriteLine(number);
        }
        Console.WriteLine("Even numbers processing completed. You can now proceed to the next operation.");

        Console.WriteLine("Grouped orders by category:");
        var orders = new List<Day1.Order>
        {
        new() { Category = "Electronics", Price = 100.00m },
        new() { Category = "Books", Price = 50.00m },
        new() { Category = "Electronics", Price = 150.00m }
    };

        var groupedOrders = Day1.DataStructuresAndLinq.GroupOrdersByCategory(orders);
        foreach (var kvp in groupedOrders)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        Console.WriteLine("Order grouping completed. You can now proceed to the next operation.");

        var students = new List<Day1.Student>
    {
        new() { Name = "Alice", Grade = 90 },
        new() { Name = "Bob", Grade = 85 },
        new() { Name = "Charlie", Grade = 95 }
    };

        var topStudents = Day1.DataStructuresAndLinq.SortStudentsByGrade(students);
        Console.WriteLine("Top 3 students by grade:");
        foreach (var student in topStudents)
        {
            Console.WriteLine($"{student.Name}: {student.Grade}");
        }

        Console.WriteLine(
            "End of the program. You can now close the console window or run another operation."
        );

        var numbersArray = new[] { 1, 2, 3, 4, 5, 2 };
        var firstDuplicate = Day2.ProblemSolving.FindFirstDuplicate(numbersArray);
        if (firstDuplicate.HasValue)
        {
            Console.WriteLine($"First duplicate number: {firstDuplicate.Value}");
        }
        else
        {
            Console.WriteLine("No duplicates found.");
        }
        Console.WriteLine("First duplicate number search completed. You can now proceed to the next operation.");

        var str1 = "listen";
        var str2 = "silent";
        var areAnagrams = Day2.ProblemSolving.AreAnagrams(str1, str2);
        Console.WriteLine(
            $"Are '{str1}' and '{str2}' anagrams? {areAnagrams}"
        );
        Console.WriteLine("Anagram check completed. You can now proceed to the next operation.");

        var text = "This is a test. This test is only a test.";
        var topWords = Day2.ProblemSolving.GetTop10CommonWords(text);
        Console.WriteLine("Top 10 common words:");
        foreach (var word in topWords)
        {
            Console.WriteLine(word);
        }
        Console.WriteLine("Top common words search completed. You can now proceed to the next operation.");

        var files = new List<Day2.FileMetadata>
    {
        new() { FilePath = "C:\\MyProjects\\CSharpInterviewPrep\\Day2\\file1.txt", FileHash = "hash1" },
        new() { FilePath = "C:\\MyProjects\\CSharpInterviewPrep\\Day2\\file2.txt", FileHash = "hash2" },
        new() { FilePath = "C:\\MyProjects\\CSharpInterviewPrep\\Day2\\file3.txt", FileHash = "hash1" }
    };
        var duplicateFiles = Day2.ProblemSolving.FindDuplicateFiles(files);
        Console.WriteLine("Duplicate files found:");
        foreach (var group in duplicateFiles)
        {
            Console.WriteLine("Group:");
            foreach (var file in group)
            {
                Console.WriteLine($"- {file}");
            }
        }
        Console.WriteLine("Duplicate file search completed. You can now proceed to the next operation.");

        var result = Day2.ProblemSolving.FindMinMax(new[] { 5, 2, 8, 1, 9 });
        Console.WriteLine($"The minimum is {result.Min}");
        Console.WriteLine($"The maximum is {result.Max}");
        Console.WriteLine("Min and Max search completed. You can now proceed to the next operation.");
        Console.WriteLine("Press any key to exit...");
    }
}