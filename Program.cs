using CSharpInterviewPrep;
using static CSharpInterviewPrep.AdvancedProblems;
using static CSharpInterviewPrep.Models.ExerciseModels;

Console.WriteLine("--- C# Interview Prep Exercises ---\n");

RunCoreCSharpConcepts();
RunProblemSolvingExercises();
RunAdvancedProblemsExercises();

Console.WriteLine("\n--- All exercises completed successfully! ---");


static void RunCoreCSharpConcepts()
{
    Console.WriteLine("--- Core C# Concepts ---");

    // ספירת תווים
    var charCounts = CoreConcepts.CountCharcters("hello world");
    Console.WriteLine($"CountCharacters('hello world'): {charCounts.Count} distinct chars found.");

    // מיון סטודנטים
    var students = new List<Student> { new() { Name = "Alice", Grade = 95 }, new() { Name = "Bob", Grade = 80 }, new() { Name = "Charlie", Grade = 92 } };
    var topStudents = CoreConcepts.SortStudentsByGrade(students);
    Console.WriteLine($"GetTopStudents: Top student is {topStudents.First().Name}.");

    Console.WriteLine("---------------------------------\n");
}

static void RunProblemSolvingExercises()
{
    Console.WriteLine("--- Problem Solving ---");

    // בדיקת אנאגרמות
    bool isAnagram = ProblemSolving.AreAnagrams("listen", "silent");
    Console.WriteLine($"AreAnagrams('listen', 'silent'): {isAnagram}");

    // מציאת כפילות ראשונה
    int? duplicate = ProblemSolving.FindFirstDuplicate(new[] { 2, 5, 1, 2, 3, 5 });
    Console.WriteLine($"FindFirstDuplicate: Found {duplicate}");

    Console.WriteLine("---------------------------------\n");
}

static void RunAdvancedProblemsExercises()
{
    Console.WriteLine("--- Advanced Problems (Product-focused) ---");

    // מסנן הרשאות
    var user = new User { MemberOfGroups = new List<string> { "Admins", "Sales" } };
    var results = new List<SearchResult>
    {
        new() { Title = "Financial Report", AllowedGroups = new List<string> { "Admins", "Finance" } },
        new() { Title = "Sales Deck", AllowedGroups = new List<string> { "Sales" } },
        new() { Title = "Dev Guide", AllowedGroups = new List<string> { "Developers" } }
    };
    var permittedResults = AdvancedProblems.FilterResultsByPermissions(results, user);
    Console.WriteLine($"FilterResultsByPermissions: User can see {permittedResults.Count} out of {results.Count} documents.");

    // השלמה אוטומטית
    var autocomplete = new AutocompleteService(new List<string> {
        "sales report Q3",
        "marketing plan",
        "salary information"
    });

    var suggestions = autocomplete.GetSuggestions("sal");
    Console.WriteLine($"Autocomplete suggestions for 'sal': Found {suggestions.Count} results.");
    foreach (var suggestion in suggestions)
    {
        Console.WriteLine($" - {suggestion}");
    }

    Console.WriteLine("---------------------------------\n");

    //AsyncDataProcessor example for demonstration
    var asyncProcessor = new AsyncDataProcessor();
    var asyncProcessUserDataAsync = asyncProcessor.ProcessUserDataAsync(123);

    asyncProcessUserDataAsync.Wait(); // Wait for the async operation to complete
    Console.WriteLine("Async operation completed. User data processed.");

    // Example of using the NaiveCounter, InterLockCounter, and LockCounter
    var naiveCounter = new NaiveCounter();
    var interLockCounter = new InterLockCounter();
    var lockCounter = new LockCounter();
    naiveCounter.Increment();
    interLockCounter.Increment();
    lockCounter.Increment();
    Console.WriteLine($"NaiveCounter: {naiveCounter.GetValue().ToString()}, InterLockCounter: {interLockCounter.GetValue().ToString()}, LockCounter: {lockCounter.GetValue().ToString()}");

    // Example of using the NotificationService with multiple senders
    var notificationSenders = new List<INotificationSender>
    {
        new EmailSender(),
        new SmsSender(),
        new WhatsAppSender()
    };

    var notificationService = new NotificationService(notificationSenders);
    var response = notificationService.SendAllNotificationsAsync("someUser", "Your order has shipped!");
    response.Wait();
}