using CSharpInterviewPrep.Exercises;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using static CSharpInterviewPrep.Exercises.A_CoreConcepts;
using static CSharpInterviewPrep.Exercises.B_ProblemSolving;
using static CSharpInterviewPrep.Exercises.C_AdvancedTopics;
using static CSharpInterviewPrep.Exercises.E_SystemDesignProblems;
using static CSharpInterviewPrep.Models.ExerciseModels;

Console.WriteLine("--- C# Interview Prep Exercises ---\n");

await RunCoreConcepts();
await RunProblemSolving();
await RunAdvancedTopics();
await RunObjectOrientedProgramming();
await RunSystemDesignProblems();

Console.WriteLine("\n--- All exercises completed successfully! ---");


static Task RunCoreConcepts()
{
    Console.WriteLine("--- Section 1: Core C# Concepts ---");

    Console.WriteLine("--- Demonstrating CountCharacters ---");
    var charCounts = CountCharacters("hello world");
    Console.WriteLine($"CountCharacters('hello world'): {charCounts.Count} distinct chars found.");

    string input = "abracadabracdb";
    char mostFrequentChar = GetMostFrequentChar(input);
    Console.WriteLine($"GetMostFrequentChar('{input}'): Most frequent char is '{mostFrequentChar}'.");

    var products = new List<Product> { new() { Id = 1, Name = "Laptop" }, new() { Id = 2, Name = "Smartphone" } };
    var productLookup = CreateProductLookup(products);
    Console.WriteLine($"CreateProductLookup: Found {productLookup.Count} products.");

    var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
    var doubledEvens = FilterAndDoubleEvens(numbers);
    Console.WriteLine($"FilterAndDoubleEvens: Found {doubledEvens.Count} even numbers, doubled them to {string.Join(", ", doubledEvens)}.");
    var orders = new List<Order>
    {
        new() { Category = "Electronics", Price = 100 },
        new() { Category = "Books", Price = 50 },
        new() { Category = "Electronics", Price = 200 }
    };
    var salesByCategory = GetSalesByCategory(orders);
    Console.WriteLine($"GetSalesByCategory: Found {salesByCategory.Count} categories with total sales.");
    foreach (var category in salesByCategory)
    {
        Console.WriteLine($"Category: {category.Key}, Total Sales: {category.Value:C}");
    }
    var students = new List<Student> { new() { Name = "Alice", Grade = 95 }, new() { Name = "Bob", Grade = 80 }, new() { Name = "Charlie", Grade = 92 } };
    var topStudents = GetTopStudents(students);
    Console.WriteLine($"GetTopStudents: Top student is {topStudents.First().Name}.");

    var base64Encoded = EncodeToBase64("Hello World");
    Console.WriteLine($"EncodeToBase64: Encoded string is {base64Encoded}.");
    var decodedText = DecodeFromBase64(base64Encoded);
    Console.WriteLine($"DecodeFromBase64: Decoded string is {decodedText}.");

    Console.WriteLine("--- Demonstrating PrintFibonacciSeries ---");
    
    PrintFibonacciSeries(10);

    Console.WriteLine("--- Demonstrating ReverseString ---");
    string original = "Hello World";
    string reversed = ReverseString(original);
    Console.WriteLine($"Original: {original}, Reversed: {reversed}");

    Console.WriteLine("--- Demonstrating IsPalindrome ---");
    string palindromeString = "";
    bool isPalindrome = IsPalindrome(palindromeString);
    Console.WriteLine($"Is '{palindromeString}' a palindrome? {isPalindrome}");

    Console.WriteLine("--- Demonstrating MergeSortedArrays ---");
    int[] array1 = { 1, 3, 5 };
    int[] array2 = { 2, 4, 6, 7 };
    int[] mergedArray = MergeSortedArrays(array1, array2);
    Console.WriteLine($"Merged array: {string.Join(", ", mergedArray)}");
    Console.WriteLine("---------------------------------\n");
    return Task.CompletedTask;
}

static Task RunProblemSolving()
{
    Console.WriteLine("--- Section 2: Problem Solving ---");
    bool isAnagram = AreAnagrams("listen", "silent");
    Console.WriteLine($"AreAnagrams('listen', 'silent'): {isAnagram}");

    int? duplicate = FindFirstDuplicate(new[] { 2, 5, 1, 2, 3 });
    Console.WriteLine($"FindFirstDuplicate: Found {duplicate}");

    string text = "This is a test. This test is only a test.";
    var topWords = GetTop10CommonWords(text);
    Console.WriteLine("GetTop10CommonWords: Top words are:");
    foreach (var word in topWords)
    {
        Console.WriteLine($"- {word}");
    }

    string rotatedArray = RotateArray(new[] { 1, 2, 3, 4, 5 }, 2);
    Console.WriteLine($"RotateArray: Rotated array is {rotatedArray}");

    Console.WriteLine("--- Demonstrating HasPairWithSum ---");
    int[] numbers = { 1, 2, 5, 4, 3 };
    int targetSum = 6;
    bool hasPair = HasPairWithSum(numbers, targetSum);
    Console.WriteLine($"HasPairWithSum: Pair with sum {targetSum} exists: {hasPair}");

    Console.WriteLine("--- Demonstrating FindSecondLargestNumber ---");
    List<int> list = [3, 1, 4, 4, 5, 2];
    int? secondLargest = FindSecondLargestNumber(list);
    Console.WriteLine($"Second largest number in {string.Join(", ", list)} is: {secondLargest}");
    Console.WriteLine("---------------------------------\n");
    return Task.CompletedTask;
}

static async Task RunAdvancedTopics()
{
    Console.WriteLine("--- Section 3: Advanced Topics ---");

    Console.WriteLine("--- Demonstrating Flexible Notification System ---");
    var emailSender = new EmailSender();
    var smsSender = new SmsSender();
    var whatsappSender = new WhatsAppSender();
    var notificationService = new NotificationService(new List<INotificationSender> { emailSender, smsSender, whatsappSender });
    await notificationService.SendAllNotificationsAsync("user123", "Your order has been shipped!");

    DemonstrateRaceCondition();

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

    var processor = new AsyncDataProcessor();
    var profile = await processor.ProcessUserDataAsync(123);
    Console.WriteLine($"Fetched profile for: {profile.Name}");

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

    var orderedProducts = GetOrderedProducts(products, orders);
    Console.WriteLine("Ordered products:");
    foreach (var product in orderedProducts)
    {
        Console.WriteLine($"- {product.Name}");
    }

    Console.WriteLine("---------------------------------\n");
}

static async Task RunObjectOrientedProgramming()
{
    Console.WriteLine("--- Section 4: Object-Oriented Programming ---");

    Console.WriteLine("--- Demonstrating Object-Oriented Programming Concepts ---");

    var shippingStrategy = ShippingStrategyFactory.GetStrategy("weight");
    var order = new Order { TotalWeight = 10.0m, Category = "Books" };
    var shippingCost = shippingStrategy.CalculateShippingCost(order);
    Console.WriteLine($"Shipping cost using WeightShippingStrategy: {shippingCost:C}");
    var configManager = ConfigurationManager.Instance;
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

    Console.WriteLine("\n--- Struct vs. Class Demo (Money Struct) ---");
    var price1 = new Money(100, "USD");
    var tax = new Money(15, "USD");

    var totalPrice = price1.Add(tax); // הפעולה יצרה אובייקט חדש

    Console.WriteLine($"Price 1: {price1}"); // price1 לא השתנה, הוא עדיין 100 USD
    Console.WriteLine($"Total Price: {totalPrice}");

    Console.WriteLine("\n--- Delegate & Event Demo (Auction) ---");

    var artPiece = new AuctionItem("Mona Lisa");
    var bidder1 = new Bidder("Alice");
    var bidder2 = new Bidder("Bob");

    // שני המציעים "נרשמים" לקבל התראות
    artPiece.NewBidPlaced += bidder1.OnNewBidPlacedHandler;
    artPiece.NewBidPlaced += bidder2.OnNewBidPlacedHandler;

    artPiece.PlaceBid("Charlie", 100); // אליס ובוב יקבלו התראה
    artPiece.PlaceBid("Alice", 150);   // רק בוב יקבל התראה
    artPiece.PlaceBid("Dave", 120);    // אף אחד לא יקבל התראה, ההצעה נמוכה מדי

    Console.WriteLine("---------------------------------\n");
}

static async Task RunSystemDesignProblems()
{
    Console.WriteLine("--- Section 5: System Design Problems ---");
    //demonstration of the Lederboard system
    Console.WriteLine("--- Demonstrating Leaderboard System ---");
    var leaderboard = new E_SystemDesignProblems.Leaderboard();
    leaderboard.UpdateScore(1, 100);
    leaderboard.UpdateScore(2, 200);

    var topPlayers = leaderboard.GetTopPlayers(1);
    Console.WriteLine($"Top player ID: {topPlayers.FirstOrDefault()}");
    //demonstration of the BadHttpService and GoodHttpService
    //Console.WriteLine("--- Demonstrating Bad and Good HTTP Services ---");
    //var badService = new BadHttpService();
    //try
    //{
    //    string badData = await badService.FetchDataAsync("http://example.com");
    //    Console.WriteLine($"Bad HTTP Service fetched data: {badData.Substring(0, 50)}...");
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine($"Bad HTTP Service error: {ex.Message}");
    //}
    //var goodService = new GoodHttpService();
    //try
    //{
    //    string goodData = await goodService.FetchDataAsync("http://example.com");
    //    Console.WriteLine($"Good HTTP Service fetched data: {goodData.Substring(0, 50)}...");
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine($"Good HTTP Service error: {ex.Message}");
    //}

    //demonstration of the LargeFileSorter
    Console.WriteLine("--- Demonstrating Large File Sorter ---");
    //var largeFileSorter = new LargeFileSorter();
    //var unsortedFilePath = "unsorted_numbers.txt"; // Assume this file exists with unsorted numbers
    //var sortedFilePath = "sorted_numbers.txt";
    //largeFileSorter.Sort(unsortedFilePath, sortedFilePath);
    //Console.WriteLine($"Large file sorted and saved to {sortedFilePath}");


    //demonstration of the ChangeDetector
    Console.WriteLine("--- Demonstrating Change Detector ---");

    var fruitV1 = new Fruit
    {
        Name = "Apple",
        Color = "Red",
        Price = 1.2m
    };

    var fruitV2 = new Fruit
    {
        Name = "Apple",
        Color = "Green",
        Price = 1.3m
    };

    var changes = ChangeDetector.FindChanges(fruitV1, fruitV2);

    Console.WriteLine("Changes detected between fruitV1 and fruitV2:");
    if (changes.Any())
    {
        foreach (var change in changes)
        {
            Console.WriteLine($"- Property '{change.PropertyName}' changed from '{change.OldValue}' to '{change.NewValue}'");
        }
    }
    else
    {
        Console.WriteLine("No changes found.");
    }


    Console.WriteLine("\n--- System Design: Thread-Safe Like Counter Demo ---");
    var likeService = new LikeCounterService();
    const int songId = 123;
    const int likesPerThread = 100_000;
    const int numberOfThreads = 10;
    const int expectedLikes = likesPerThread * numberOfThreads;

    var threads = new List<Thread>();

    for (int i = 0; i < numberOfThreads; i++)
    {
        var thread = new Thread(() =>
        {
            for (int j = 0; j < likesPerThread; j++)
            {
                likeService.Like(songId);
            }
        });
        threads.Add(thread);
        thread.Start();
    }

    foreach (var thread in threads)
    {
        thread.Join(); // המתן לסיום כל ה-Threads
    }

    int finalCount = likeService.GetLikeCount(songId);
    Console.WriteLine($"Final like count for song {songId}: {finalCount:N0}");
    Console.WriteLine($"Expected like count: {expectedLikes:N0}");
    Console.WriteLine($"Success: {finalCount == expectedLikes}");
    Console.WriteLine("---------------------------------\n");
}