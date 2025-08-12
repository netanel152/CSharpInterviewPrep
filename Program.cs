using CSharpInterviewPrep.Exercises;
using Microsoft.Extensions.DependencyInjection;
using static CSharpInterviewPrep.Models.ExerciseModels;

Console.WriteLine("--- C# Interview Prep Exercises ---\n");

// Run all exercise sections in a logical order
await RunCoreConcepts();
await RunProblemSolving();
await RunAdvancedTopics();

Console.WriteLine("\n--- All exercises completed successfully! ---");


static Task RunCoreConcepts()
{
    Console.WriteLine("--- Section 1: Core C# Concepts ---");
    var charCounts = A_CoreConcepts.CountCharacters("hello world");
    Console.WriteLine($"CountCharacters('hello world'): {charCounts.Count} distinct chars found.");

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
    Console.WriteLine("---------------------------------\n");
    return Task.CompletedTask;
}

static async Task RunAdvancedTopics()
{
    Console.WriteLine("--- Section 3: Advanced Topics ---");

    // Asynchronous Processing Demo
    var processor = new AsyncDataProcessor();
    var profile = await processor.ProcessUserDataAsync(123);
    Console.WriteLine($"Fetched profile for: {profile.Name}");

    // Dependency Injection & OOP Demo
    var services = new ServiceCollection();
    services.AddTransient<INotificationSender, EmailSender>();
    services.AddTransient<INotificationSender, SmsSender>();
    services.AddTransient<NotificationService>();
    var serviceProvider = services.BuildServiceProvider();

    var notificationService = serviceProvider.GetRequiredService<NotificationService>();
    await notificationService.SendAllNotificationsAsync("Natanel", "Your interview is confirmed!");

    Console.WriteLine("---------------------------------\n");
}