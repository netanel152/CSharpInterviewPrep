using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

// --- Part 1: Concurrency - Thread Safety ---
public class NaiveCounter
{
    private int _value = 0;
    public void Increment()
    {
        _value++;
    }
    public int GetValue()
    {
        return _value;
    }
}
public class LockCounter
{
    private readonly Lock _lock = new();
    private int _value = 0;
    public void Increment()
    {
        lock (_lock) { _value++; }
    }
    public int GetValue()
    {
        lock (_lock) { return _value; }
    }
}
public class InterlockedCounter
{
    private int _value = 0;
    public void Increment()
    {
        Interlocked.Increment(ref _value);
    }
    public int GetValue()
    {
        return Interlocked.CompareExchange(ref _value, 0, 0);
    }
}

// --- Part 2: Concurrency - Asynchronous Programming ---
public class AsyncDataProcessor
{
    private async Task<string> GetUserNameAsync(int userId)
    {
        await Task.Delay(1000); return "Natanel Doe";
    }

    private async Task<List<string>> GetUserPermissionsAsync(int userId)
    {
        await Task.Delay(1500); return new List<string> { "read", "write" };
    }

    public async Task<UserProfile> ProcessUserDataAsync(int userId)
    {
        var stopwatch = Stopwatch.StartNew();
        var nameTask = GetUserNameAsync(userId);
        var permissionsTask = GetUserPermissionsAsync(userId);

        await Task.WhenAll(nameTask, permissionsTask);

        stopwatch.Stop();
        Console.WriteLine($"Total async processing time: {stopwatch.ElapsedMilliseconds} ms (should be ~1500ms)");

        return new UserProfile { Name = await nameTask, Permissions = await permissionsTask };
    }
}

// --- Part 3: OOP & System Design - Flexible Notification System ---
public interface INotificationSender
{
    Task SendAsync(string userId, string message);
}

public class EmailSender : INotificationSender
{
    public Task SendAsync(string userId, string message)
    {
        Console.WriteLine($"Email sent to {userId}: {message}");
        return Task.CompletedTask;
    }
}

public class SmsSender : INotificationSender
{
    public Task SendAsync(string userId, string message)
    {
        Console.WriteLine($"SMS sent to {userId}: {message}"); 
        return Task.CompletedTask;
    }
}
public class WhatsAppSender : INotificationSender
{
    public Task SendAsync(string userId, string message)
    {
        Console.WriteLine($"WhatsApp sent to {userId}: {message}");
        return Task.CompletedTask;
    }
}

public class NotificationService
{
    private readonly IEnumerable<INotificationSender> _senders;
    public NotificationService(IEnumerable<INotificationSender> senders)
    {
        _senders = senders;
    }
    public async Task SendAllNotificationsAsync(string userId, string message)
    {
        await Task.WhenAll(_senders.Select(s => s.SendAsync(userId, message)));
    }
}

// --- Part 4: Design Patterns - Caching Decorator ---
public interface IRepository { Task<string> GetById(int id); }

public class SlowRepository : IRepository // A "real" repository that is slow
{
    public async Task<string> GetById(int id)
    {
        Console.WriteLine($"--> Querying database for ID: {id}...");
        await Task.Delay(2000); // Simulate slow DB call
        return $"Data for {id}";
    }
}

public class CachingRepository : IRepository // The decorator
{
    private readonly IRepository _decorated;
    private readonly IMemoryCache _cache;
    private MemoryCacheEntryOptions _cacheOptions;


    public CachingRepository(IRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<string> GetById(int id)
    {
        string cacheKey = $"data_{id}";
        var cachedData = _cache.Get<string>(cacheKey);
        if (cachedData != null)
        {
            Console.WriteLine($"--> Cache hit for ID: {id}");
            return cachedData;
        }
        Console.WriteLine($"--> Cache miss for ID: {id}");
        var data = await _decorated.GetById(id);
        _cache.Set(cacheKey, data, _cacheOptions);
        return data;
    }
}

public static class C_AdvancedTopics
{
    public static void DemonstrateRaceCondition()
    {
        Console.WriteLine("\n--- Demonstrating Race Condition ---");
        var naiveCounter = new NaiveCounter();
        RunCounterInParallel(naiveCounter.Increment);
        Console.WriteLine($"Naive Counter Final Value: {naiveCounter.GetValue()} (Incorrect!)");

        var interLockCounter = new InterlockedCounter();
        RunCounterInParallel(interLockCounter.Increment);
        Console.WriteLine($"Interlocked Counter Final Value: {interLockCounter.GetValue()} (Correct!)");

        var lockCounter = new LockCounter();
        RunCounterInParallel(lockCounter.Increment);
        Console.WriteLine($"Lock Counter Final Value: {lockCounter.GetValue()} (Correct!)");
    }

    private static void RunCounterInParallel(Action incrementAction)
    {
        const int numThreads = 10;
        const int incrementsPerThread = 100_000;
        var threads = new List<Thread>();

        for (int i = 0; i < numThreads; i++)
        {
            var thread = new Thread(() =>
            {
                for (int j = 0; j < incrementsPerThread; j++) { incrementAction(); }
            });
            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    public static IEnumerable<Product> GetOrderedProducts(List<Product> allProducts, List<Order> allOrders)
    {
        var orderedProducts = new HashSet<int>(allOrders.Select(o => o.ProductId));
        return allProducts.Where(p => orderedProducts.Contains(p.Id));
    }

}