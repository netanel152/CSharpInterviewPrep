using CSharpInterviewPrep.Exercises;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using static CSharpInterviewPrep.Models.ExerciseModels;

Console.WriteLine("--- C# Interview Prep Exercises ---\n");

await RunCoreConcepts();
await RunProblemSolving();
await RunAdvancedTopics();

Console.WriteLine("\n--- All exercises completed successfully! ---");


static Task RunCoreConcepts()
{
    Console.WriteLine("--- Section 1: Core C# Concepts ---");
    var charCounts = A_CoreConcepts.CountCharacters("hello world");
    Console.WriteLine($"CountCharacters('hello world'): {charCounts.Count} distinct chars found.");

    var products = new List<Product> { new() { Id = 1, Name = "Laptop" }, new() { Id = 2, Name = "Smartphone" } };
    var productLookup = A_CoreConcepts.CreateProductLookup(products);
    Console.WriteLine($"CreateProductLookup: Found {productLookup.Count} products.");

    var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
    var doubledEvens = A_CoreConcepts.FilterAndDoubleEvens(numbers);
    Console.WriteLine($"FilterAndDoubleEvens: Found {doubledEvens.Count} even numbers, doubled them to {string.Join(", ", doubledEvens)}.");
    var orders = new List<Order>
    {
        new() { Category = "Electronics", Price = 100 },
        new() { Category = "Books", Price = 50 },
        new() { Category = "Electronics", Price = 200 }
    };
    var salesByCategory = A_CoreConcepts.GetSalesByCategory(orders);
    Console.WriteLine($"GetSalesByCategory: Found {salesByCategory.Count} categories with total sales.");
    foreach (var category in salesByCategory)
    {
        Console.WriteLine($"Category: {category.Key}, Total Sales: {category.Value:C}");
    }
    var students = new List<Student> { new() { Name = "Alice", Grade = 95 }, new() { Name = "Bob", Grade = 80 }, new() { Name = "Charlie", Grade = 92 } };
    var topStudents = A_CoreConcepts.GetTopStudents(students);
    Console.WriteLine($"GetTopStudents: Top student is {topStudents.First().Name}.");
    Console.WriteLine("---------------------------------\n");
    return Task.CompletedTask;
}

static Task RunProblemSolving()
{
    Console.WriteLine("--- Section 2: Problem Solving ---");
    bool isAnagram = B_ProblemSolving.AreAnagrams("listen", "silent");
    Console.WriteLine($"AreAnagrams('listen', 'silent'): {isAnagram}");

    int? duplicate = B_ProblemSolving.FindFirstDuplicate(new[] { 2, 5, 1, 2, 3 });
    Console.WriteLine($"FindFirstDuplicate: Found {duplicate}");

    string text = "This is a test. This test is only a test.";
    var topWords = B_ProblemSolving.GetTop10CommonWords(text);
    Console.WriteLine("GetTop10CommonWords: Top words are:");
    foreach (var word in topWords)
    {
        Console.WriteLine($"- {word}");
    }
    Console.WriteLine("---------------------------------\n");
    return Task.CompletedTask;
}

static async Task RunAdvancedTopics()
{
    Console.WriteLine("--- Section 3: Advanced Topics ---");

    // Flexible Notification System Demo
    Console.WriteLine("--- Demonstrating Flexible Notification System ---");
    var emailSender = new EmailSender();
    var smsSender = new SmsSender();
    var whatsappSender = new WhatsAppSender();
    var notificationService = new NotificationService(new List<INotificationSender> { emailSender, smsSender, whatsappSender });
    await notificationService.SendAllNotificationsAsync("user123", "Your order has been shipped!");


    // Race Condition Demo
    C_AdvancedTopics.DemonstrateRaceCondition();
    Console.WriteLine("---------------------------------\n");

    // Caching Decorator Pattern Demo
    Console.WriteLine("--- Demonstrating Caching Decorator Pattern ---");
    var cache = new MemoryCache(new MemoryCacheOptions());
    IRepository repository = new CachingRepository(new SlowRepository(), cache);

    Console.WriteLine("First call (should be slow):");
    var stopwatch = Stopwatch.StartNew();
    stopwatch.Start();
    await repository.GetById(123);
    stopwatch.Stop();
    Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine("\nSecond call (should be fast and from cache):");
    stopwatch.Restart();
    await repository.GetById(123);
    stopwatch.Stop();
    Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

    Console.WriteLine("\nWaiting 10 seconds for cache to expire...");
    await Task.Delay(10000);

    Console.WriteLine("\nThird call after expiration (should be slow again):");
    stopwatch.Restart();
    await repository.GetById(123);
    stopwatch.Stop();
    Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine("---------------------------------\n");

    // Asynchronous Processing Demo
    var processor = new AsyncDataProcessor();
    var profile = await processor.ProcessUserDataAsync(123);
    Console.WriteLine($"Fetched profile for: {profile.Name}");
    Console.WriteLine("---------------------------------\n");
}