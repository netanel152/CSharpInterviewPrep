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

    var base64Encoded = A_CoreConcepts.EncodeToBase64("Hello World");
    Console.WriteLine($"EncodeToBase64: Encoded string is {base64Encoded}.");
    var decodedText = A_CoreConcepts.DecodeFromBase64(base64Encoded);
    Console.WriteLine($"DecodeFromBase64: Decoded string is {decodedText}.");
    Console.WriteLine("---------------------------------\n");

    //dmonstration of the PrintFibonacciSeries
    Console.WriteLine("--- Demonstrating PrintFibonacciSeries ---");
    A_CoreConcepts.PrintFibonacciSeries(10);

    // demonstration of the ReverseString
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

    string rotatedArray = B_ProblemSolving.RotateArray(new[] { 1, 2, 3, 4, 5 }, 2);
    Console.WriteLine($"RotateArray: Rotated array is {rotatedArray}");
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

    // Demonstrating GetOrderedProducts
    Console.WriteLine("--- Demonstrating GetOrderedProducts ---");
    var products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop" },
        new Product { Id = 2, Name = "Smartphone"},
        new Product { Id = 3, Name = "Tablet" }
    };
    var orders = new List<Order>
    {
        new Order { ProductId = 1, Category = "Electronics", Price = 1000 },
        new Order { ProductId = 1, Category = "Electronics", Price = 500 },
        new Order { ProductId = 3, Category = "Electronics", Price = 300 }
    };

    var orderedProducts = C_AdvancedTopics.GetOrderedProducts(products, orders);
    Console.WriteLine("Ordered products:");
    foreach (var product in orderedProducts)
    {
        Console.WriteLine($"- {product.Name}");
    }

    // demonstation of object oriented programming concepts
    Console.WriteLine("--- Demonstrating Object-Oriented Programming Concepts ---");

    var shippingStrategy = ShippingStrategyFactory.GetStrategy("weight");
    var order = new Order { TotalWeight = 10.0m, Category = "Books" };
    var shippingCost = shippingStrategy.CalculateShippingCost(order);
    Console.WriteLine($"Shipping cost using WeightShippingStrategy: {shippingCost:C}");
    //var configManager = ConfigurationManager.Instance;
    //Console.WriteLine($"ConfigurationManager instance created with settings: {string.Join(", ", configManager.Settings.Select(kv => $"{kv.Key}={kv.Value}"))}"); 
    //Console.WriteLine($"ConfigurationManager instance created with settings: {configManager.Settings.Count} settings loaded.");
    //Console.WriteLine($"DatabaseConnectionString: {configManager.Settings["DatabaseConnectionString"]}");
    //Console.WriteLine($"ApiEndpoint: {configManager.Settings["ApiEndpoint"]}");
    //Console.WriteLine($"CacheTimeout: {configManager.Settings["CacheTimeout"]} seconds");

    // demonstration of the LegacyLogger
    Console.WriteLine("--- Demonstrating Legacy Logger ---");
    var legacyLogger = new LegacyLogger();
    legacyLogger.WriteEntry("This is a log message from the legacy logger.");


    Console.WriteLine("Legacy logger has been successfully used to log a message.");

    // demonstration of the ProductEvent
    var productEvent = new ProductEvent { Price = 100 };
    var customer1 = new Customer();

    // הלקוח "נרשם" לקבל התראות על שינויים במחיר
    productEvent.PriceChanged += customer1.OnPriceChanged;

    // שינוי המחיר יפעיל אוטומטית את המתודה של הלקוח
    productEvent.Price = 90;

    Console.WriteLine("---------------------------------\n");
}