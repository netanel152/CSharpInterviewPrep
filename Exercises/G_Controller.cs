using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // Added EF Core namespace
using Microsoft.Extensions.Logging;

namespace CSharpInterviewPrep.Exercises
{
    // --- Mocking ASP.NET Core Environment ---
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

    // --- Database & Models ---
    public class DB_User 
    { 
        public int Id { get; set; } 
        public required string Email { get; set; } 
    } 
    
    public class DB_Product 
    { 
        public int Id { get; set; } 
        public decimal Price { get; set; } 
        public int Stock { get; set; } 
    } 
    
    public class DB_Order 
    { 
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public decimal Total { get; set; } 
        public DateTime Date { get; set; } 
    } 

    // ✅ REFACTORED: Using Real Entity Framework Core DbContext
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DB_User> Users { get; set; } 
        public DbSet<DB_Product> Products { get; set; } 
        public DbSet<DB_Order> Orders { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Data
            modelBuilder.Entity<DB_User>().HasData(
                new DB_User { Id = 1, Email = "test@test.com" }
            );

            modelBuilder.Entity<DB_Product>().HasData(
                new DB_Product { Id = 101, Price = 100, Stock = 5 },
                new DB_Product { Id = 102, Price = 50, Stock = 0 }
            );
        }
    }

    public interface INotificationService
    {
        Task NotifyMarketingSystemAsync(int userId, decimal amount);
    }

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
            Console.WriteLine($"[Async] Notification sent to Marketing OS for User {userId}.");
            await Task.CompletedTask;
        }
    }

    public class OrdersController
    {
        private readonly AppDbContext _dbContext;
        private readonly INotificationService _notificationService;
        private readonly Microsoft.Extensions.Logging.ILogger<OrdersController> _logger;

        public OrdersController(AppDbContext dbContext, INotificationService notificationService, Microsoft.Extensions.Logging.ILogger<OrdersController> logger)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _logger = logger;
        }

        // 1. GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            _logger.LogInformation("[GET] Fetching all orders...");
            // Real EF Core async call
            var orders = await _dbContext.Orders.ToListAsync(); 
            _logger.LogInformation("Found {Count} orders.", orders.Count);
            return new OkObjectResult(orders);
        }

        // 2. GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            _logger.LogInformation("[GET] Fetching order {Id}...", id);
            var order = await _dbContext.Orders.FindAsync(id);
            
            if (order == null)
            {
                _logger.LogWarning("Order {Id} not found.", id);
                return new NotFoundResult();
            }

            _logger.LogInformation("Order {Id} found. Total: {Total:C}", id, order.Total);
            return new OkObjectResult(order);
        }

        // 3. CREATE (POST)
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderAsync(int userId, List<int> productIds)
        {
            _logger.LogInformation("[POST] Creating Order for User {UserId} with products: {ProductIds}", userId, string.Join(",", productIds));

            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found.", userId);
                    return new BadRequestResult();
                }

                // Fetch products (EF Core translates this to IN clause)
                var products = await _dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                decimal total = 0;

                foreach (var pid in productIds)
                {
                    var product = products.FirstOrDefault(p => p.Id == pid);
                    if (product == null || product.Stock <= 0)
                    {
                        _logger.LogWarning("Product {ProductId} is unavailable/out of stock. Skipping.", pid);
                        continue;
                    }

                    total += product.Price;
                    product.Stock--; 
                    // EF Change Tracking will detect this modification automatically
                }

                if (total == 0)
                {
                    _logger.LogWarning("Order total is 0. Aborting.");
                    return new BadRequestResult();
                }

                var order = new DB_Order 
                { 
                    // Id is auto-generated by EF Core (Identity)
                    UserId = userId, 
                    Total = total, 
                    Date = DateTime.Now 
                };
                
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync(); // Atomic Transaction

                _logger.LogInformation("✅ Order created! ID: {OrderId}, Total: {Total:C}", order.Id, total);

                await _notificationService.NotifyMarketingSystemAsync(userId, total);

                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CRITICAL ERROR creating order for user {UserId}", userId);
                return new BadRequestResult();
            }
        }

        // 4. UPDATE (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(int id, decimal newTotal)
        {
            _logger.LogInformation("[PUT] Updating Order {OrderId} with new total {NewTotal:C}...", id, newTotal);
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found.", id);
                return new NotFoundResult();
            }

            order.Total = newTotal;
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Order {OrderId} updated successfully.", id);
            return new OkObjectResult(order);
        }

        // 5. DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            _logger.LogInformation("[DELETE] Deleting Order {OrderId}...", id);
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found.", id);
                return new NotFoundResult();
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} deleted successfully.", id);
            return new OkResult();
        }
    }

    public static class G_ControllerChallenge
    {
        public static void Run()
        {
            Console.WriteLine("--- Section 7: Controller Refactoring (Real EF Core + CRUD + Logging) ---");

            var services = new ServiceCollection();
            
            // ✅ Setup Real EF Core with InMemory Database
            services.AddDbContext<AppDbContext>(options => 
                options.UseInMemoryDatabase("InterviewDB"));

            // ✅ Add Logging
            services.AddLogging(configure => configure.AddConsole());

            services.AddHttpClient(); 
            services.AddSingleton<INotificationService, MarketingNotificationService>();
            services.AddScoped<OrdersController>(); // Controllers are usually Scoped

            var provider = services.BuildServiceProvider();

            // ✅ Ensure Database Created & Seeded
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                
                var controller = scope.ServiceProvider.GetRequiredService<OrdersController>();

                // 1. Create
                controller.CreateOrderAsync(1, new List<int> { 101, 102, 999 }).Wait();

                // 2. Get All
                controller.GetAllOrdersAsync().Wait();

                // 3. Update (Id 1 created above)
                controller.UpdateOrderAsync(1, 200.00m).Wait();

                // 4. Get By Id
                controller.GetOrderByIdAsync(1).Wait();

                // 5. Delete
                controller.DeleteOrderAsync(1).Wait();

                // 6. Verify Deletion
                controller.GetOrderByIdAsync(1).Wait();
            }

            Console.WriteLine("---------------------------------------------------\n");
        }
    }
}
