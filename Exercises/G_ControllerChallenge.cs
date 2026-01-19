using Microsoft.Extensions.DependencyInjection;

namespace CSharpInterviewPrep.Exercises
{
    // Mocking ASP.NET Core Environment (כדי שהקוד יתקמפל ב-Console App)
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : Attribute { public HttpPostAttribute(string route) { } } 
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : Attribute { public HttpGetAttribute(string route = "") { } } 
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : Attribute { public HttpPutAttribute(string route) { } } 
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : Attribute { public HttpDeleteAttribute(string route) { } } 

    public interface IActionResult { } 
    public class OkResult : IActionResult { } 
    public class OkObjectResult : IActionResult { public object Value { get; } public OkObjectResult(object value) { Value = value; } } 
    public class BadRequestResult : IActionResult { } 
    public class NotFoundResult : IActionResult { } 

    // Database & Models Simulation
    public class DB_User { public int Id { get; set; } public required string Email { get; set; } } 
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
        private int _orderIdCounter = 1;

        public int GenerateOrderId() => _orderIdCounter++;

        public void SaveChanges() { Console.WriteLine("--> DB Changes Saved (Simulated)"); }
    }

    public interface INotificationService
    {
        Task NotifyMarketingSystemAsync(int userId, decimal amount);
    }

    // Notification Service
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

    public class OrdersController
    {
        private readonly FakeDbContext _dbContext;
        private readonly INotificationService _notificationService;

        // Dependency Injection
        public OrdersController(FakeDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            Console.WriteLine("\n[GET] Fetching all orders...");
            // In real app: Use AutoMapper to map to DTOs
            var orders = _dbContext.Orders.ToList();
            await Task.CompletedTask; 
            Console.WriteLine($"Found {orders.Count} orders.");
            return new OkObjectResult(orders);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            Console.WriteLine($"\n[GET] Fetching order {id}...");
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == id);
            
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return new NotFoundResult();
            }

            Console.WriteLine($"Order {id} found. Total: {order.Total:C}");
            await Task.CompletedTask;
            return new OkObjectResult(order);
        }

        // CREATE (POST)
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderAsync(int userId, List<int> productIds)
        {
            Console.WriteLine($"\n[POST] Creating Order for User {userId}...");

            try
            {
                // Validate User
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return new BadRequestResult();
                }

                // Fetch all products (Solves N+1)
                var products = _dbContext.Products.Where(p => productIds.Contains(p.Id)).ToList();
                decimal total = 0;

                // Process Logic
                foreach (var pid in productIds)
                {
                    var product = products.FirstOrDefault(p => p.Id == pid);
                    if (product == null || product.Stock <= 0)
                    {
                        Console.WriteLine($"Warning: Product {pid} is unavailable/out of stock. Skipping.");
                        continue;
                    }

                    total += product.Price;
                    product.Stock--; 
                }

                if (total == 0)
                {
                    Console.WriteLine("Order total is 0. Aborting.");
                    return new BadRequestResult();
                }

                // Save
                var order = new DB_Order 
                { 
                    Id = _dbContext.GenerateOrderId(),
                    UserId = userId, 
                    Total = total, 
                    Date = DateTime.Now 
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                Console.WriteLine($"✅ Order created! ID: {order.Id}, Total: {total:C}");

                // Notify
                await _notificationService.NotifyMarketingSystemAsync(userId, total);

                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
                return new BadRequestResult();
            }
        }

        // UPDATE (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(int id, decimal newTotal)
        {
            Console.WriteLine($"\n[PUT] Updating Order {id} with new total {newTotal:C}...");
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return new NotFoundResult();
            }

            order.Total = newTotal;
            _dbContext.SaveChanges();
            
            Console.WriteLine("Order updated successfully.");
            await Task.CompletedTask;
            return new OkObjectResult(order);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            Console.WriteLine($"\n[DELETE] Deleting Order {id}...");
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return new NotFoundResult();
            }

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();

            Console.WriteLine("Order deleted successfully.");
            await Task.CompletedTask;
            return new OkResult();
        }
    }

    public static class G_ControllerChallenge
    {
        public static void Run()
        {
            Console.WriteLine("--- Section 7: Controller Refactoring (Full CRUD) ---");

            // Setup Dependency Injection Container
            var services = new ServiceCollection();
            services.AddSingleton<FakeDbContext>();
            services.AddHttpClient(); 
            services.AddSingleton<INotificationService, MarketingNotificationService>();
            services.AddSingleton<OrdersController>();

            var provider = services.BuildServiceProvider();
            var controller = provider.GetRequiredService<OrdersController>();

            // Create
            controller.CreateOrderAsync(1, new List<int> { 101, 102, 999 }).Wait();

            // Get All
            controller.GetAllOrdersAsync().Wait();

            // Update (Assume ID 1 was created)
            controller.UpdateOrderAsync(1, 200.00m).Wait();

            // Get By Id
            controller.GetOrderByIdAsync(1).Wait();

            // Delete
            controller.DeleteOrderAsync(1).Wait();

            // Verify Deletion
            controller.GetOrderByIdAsync(1).Wait();

            Console.WriteLine("---------------------------------------------------\n");
        }
    }
}