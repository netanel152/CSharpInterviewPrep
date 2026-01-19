using System.Diagnostics;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

public static class C_AdvancedTopics
{
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
            await Task.Delay(1500);
            return new List<string> { "read", "write" };
        }

        public async Task<UserProfile> ProcessUserDataAsync(int userId)
        {
            var stopwatch = Stopwatch.StartNew();
            var nameTask = GetUserNameAsync(userId);
            var permissionsTask = GetUserPermissionsAsync(userId);

            await Task.WhenAll(nameTask, permissionsTask);

            stopwatch.Stop();
            Console.WriteLine($"Total async processing time: {stopwatch.ElapsedMilliseconds} ms (should be ~1500ms)");

            return new UserProfile
            {
                Name = await nameTask,
                Permissions = await permissionsTask
            };
        }
    }

    public static IEnumerable<Product> GetOrderedProducts(List<Product> allProducts, List<Order> allOrders)
    {
        var orderedProducts = new HashSet<int>(allOrders.Select(o => o.ProductId));
        return allProducts.Where(p => orderedProducts.Contains(p.Id));
    }

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

    public static async Task Run()
    {
        Console.WriteLine("--- Section 3: Advanced Topics ---");

        DemonstrateRaceCondition();

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
}