using System.Runtime.CompilerServices;

namespace CSharpInterviewPrep.Exercises;

public static class M_AsyncStreams
{
    
    // Scenario: Streaming Big Data to Frontend    
    // In a "Big Data" environment, you often need to send large datasets 
    // to the client (React) without loading everything into memory (RAM) or blocking the request.
    // IAsyncEnumerable allows you to "stream" data item by item as it becomes available.

    // Simulating a database fetch that takes time per item
    public static async IAsyncEnumerable<int> FetchLargeDatasetAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int i = 1; i <= 5; i++)
        {
            // Simulate async I/O work (DB query, External API call)
            await Task.Delay(500, cancellationToken); 
            
            yield return i; // "Push" data to the caller immediately
        }
    }

    // Simulating a Consumer (e.g., an API Controller or a Background Service)
    public static async Task ConsumeStreamAsync()
    {
        Console.WriteLine("1. Consuming Async Stream (IAsyncEnumerable):");
        var timer = System.Diagnostics.Stopwatch.StartNew();

        await foreach (var number in FetchLargeDatasetAsync())
        {
            Console.WriteLine($"   -> Received item {number} at {timer.Elapsed.TotalSeconds:F1}s");
            // In a real API, you would write this to the Response.Body stream immediately.
        }
        Console.WriteLine("   Stream finished.");
    }

    // Scenario: Throttling / Rate Limiting Parallel Requests
    // When dealing with many external APIs (Insurance providers), you can't just 
    // fire 1000 requests at once. You need SemaphoreSlim to throttle.

    public static async Task ThrottledExecutionDemo()
    {
        Console.WriteLine("\n2. Throttling Parallel Requests (SemaphoreSlim):");
        
        // Allow max 2 concurrent requests
        using var semaphore = new SemaphoreSlim(2);
        var tasks = new List<Task>();

        for (int i = 1; i <= 5; i++)
        {
            int id = i;
            tasks.Add(Task.Run(async () =>
            {
                Console.WriteLine($"   [Task {id}] Waiting to enter...");
                await semaphore.WaitAsync(); // Decrement counter
                try
                {
                    Console.WriteLine($"   [Task {id}] PROCESSING (Inside Critical Section)");
                    await Task.Delay(1000); // Simulate work
                }
                finally
                {
                    Console.WriteLine($"   [Task {id}] Released.");
                    semaphore.Release(); // Increment counter
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    public static async Task Run()
    {
        Console.WriteLine("--- Section 13: Async Streams & Concurrency Control ---");
        await ConsumeStreamAsync();
        await ThrottledExecutionDemo();
        Console.WriteLine("-------------------------------------------------------\n");
    }
}
