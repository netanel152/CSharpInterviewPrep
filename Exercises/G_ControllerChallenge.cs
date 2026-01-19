using Microsoft.Extensions.DependencyInjection;

namespace CSharpInterviewPrep.Exercises
{
    // --- Mocking ASP.NET Core Environment (כדי שהקוד יתקמפל ב-Console App) ---
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : Attribute { public HttpPostAttribute(string route) { } }
    public interface IActionResult { }
    public class OkResult : IActionResult { }
    public class BadRequestResult : IActionResult { }

    // --- Database & Models Simulation ---
    public class DB_User { public int Id { get; set; } public string Email { get; set; } }
    public class DB_Product { public int Id { get; set; } public decimal Price { get; set; } public int Stock { get; set; } }
    public class DB_Order { public int Id { get; set; } public int UserId { get; set; } public decimal Total { get; set; } public DateTime Date { get; set; } }

    public class FakeDbContext
    {
        public List<DB_User> Users = new() { new DB_User { Id = 1, Email = "test@test.com" } };
        public List<DB_Product> Products = new()
        {
            new DB_Product { Id = 101, Price = 100, Stock = 5 },
            new DB_Product { Id = 102, Price = 50, Stock = 0 }
        };
        public List<DB_Order> Orders = new();

        public void SaveChanges() { Console.WriteLine("--> DB Changes Saved (Simulated)"); }
    }

    public interface INotificationService
    {
        Task NotifyMarketingSystemAsync(int userId, decimal amount);
    }

    // --- Correct Implementation of Notification Service ---
    public class MarketingNotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MarketingNotificationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task NotifyMarketingSystemAsync(int userId, decimal amount)
        {
            var client = _httpClientFactory.CreateClient();
            // In real code: await client.PostAsync(...)
            Console.WriteLine($"[Async] Notification sent to Marketing OS for User {userId}.");
            await Task.CompletedTask;
        }
    }

    // ==========================================
    //           THE REFACTORED CONTROLLER
    // ==========================================
    public class OrderControllerRefactored // <--- הוספתי את הגדרת המחלקה שהייתה חסרה
    {
        private readonly FakeDbContext _db;
        private readonly INotificationService _notificationService;

        // ✅ Dependency Injection
        public OrderControllerRefactored(FakeDbContext db, INotificationService notificationService)
        {
            _db = db;
            _notificationService = notificationService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderAsync(int userId, List<int> productIds)
        {
            Console.WriteLine($"\n--- Processing Order for User {userId} (Refactored) ---");

            try
            {
                // 1. Validate User (Simulated Async)
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return new BadRequestResult();
                }

                // 2. Fetch all products in ONE query (Solves N+1)
                var products = _db.Products.Where(p => productIds.Contains(p.Id)).ToList();

                decimal total = 0;
                var processedProducts = new List<DB_Product>();

                // 3. Process Logic In-Memory
                foreach (var pid in productIds)
                {
                    var product = products.FirstOrDefault(p => p.Id == pid);
                    if (product == null || product.Stock <= 0)
                    {
                        Console.WriteLine($"Warning: Product {pid} is unavailable/out of stock. Skipping.");
                        continue;
                    }

                    total += product.Price;
                    product.Stock--; // Modify in memory
                    processedProducts.Add(product);
                }

                if (total == 0)
                {
                    Console.WriteLine("Order total is 0. Aborting.");
                    return new BadRequestResult();
                }

                // 4. Save Changes Atomically
                var order = new DB_Order { UserId = userId, Total = total, Date = DateTime.Now };
                _db.Orders.Add(order);

                _db.SaveChanges();

                Console.WriteLine($"✅ Order successfully created! ID: {order.Id}, Total: {total}");

                // 5. Fire and Forget (or Await depending on business logic)
                await _notificationService.NotifyMarketingSystemAsync(userId, total);

                return new OkResult();
            }
            catch (Exception ex)
            {
                // ✅ Log the error properly
                Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
                return new BadRequestResult();
            }
        }
    }

    public static class G_ControllerChallenge
    {
        public static void Run()
        {
            Console.WriteLine("--- Section 7: Controller Refactoring Challenge ---");

            // Setup Dependency Injection Container (Simulation)
            var services = new ServiceCollection();
            services.AddSingleton<FakeDbContext>();
            services.AddHttpClient(); // Registers IHttpClientFactory
            services.AddSingleton<INotificationService, MarketingNotificationService>();
            services.AddSingleton<OrderControllerRefactored>();

            var provider = services.BuildServiceProvider();

            // Run the controller
            var controller = provider.GetRequiredService<OrderControllerRefactored>();

            // Scenario: 101 exists, 102 out of stock, 999 doesn't exist
            controller.CreateOrderAsync(1, new List<int> { 101, 102, 999 }).Wait();

            Console.WriteLine("---------------------------------------------------\n");
        }
    }
}