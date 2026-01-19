using CSharpInterviewPrep.Exercises;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CSharpInterviewPrep.Tests;

public class OrdersControllerTests
{
    private readonly AppDbContext _dbContext;
    private readonly OrdersController _controller;
    // Mock logger using a simple wrapper or NullLogger if preferred, 
    // but here we use a simple stub since we don't assert logs in unit tests typically.
    private readonly Microsoft.Extensions.Logging.ILogger<OrdersController> _logger;

    public OrdersControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
            .Options;

        _dbContext = new AppDbContext(options);
        _dbContext.Database.EnsureCreated();

        // Stub logger
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = factory.CreateLogger<OrdersController>();

        // Stub Notification Service
        var notificationService = new StubNotificationService();

        _controller = new OrdersController(_dbContext, notificationService, _logger);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnOk_WhenValid()
    {
        // Arrange
        int userId = 1;
        var productIds = new List<int> { 101, 101 }; // 2 items of ID 101 ($100 each)

        // Act
        var result = await _controller.CreateOrderAsync(userId, productIds);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var order = Assert.IsType<DB_Order>(okResult.Value);
        
        Assert.Equal(200, order.Total); // 100 + 100
        Assert.Equal(userId, order.UserId);
        
        // Verify DB side effect
        Assert.Equal(3, _dbContext.Products.Find(101)!.Stock); // Was 5, bought 2 -> 3
    }

    [Fact]
    public async Task CreateOrder_ShouldFail_WhenProductOutOfStock()
    {
        // Arrange
        int userId = 1;
        var productIds = new List<int> { 102 }; // ID 102 has 0 stock

        // Act
        var result = await _controller.CreateOrderAsync(userId, productIds);

        // Assert
        // In our logic, it skips the item. If total is 0, it returns BadRequest.
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnOrder_WhenExists()
    {
        // Arrange
        var order = new DB_Order { UserId = 1, Total = 500, Date = DateTime.Now };
        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();

        // Act
        var result = await _controller.GetOrderByIdAsync(order.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrder = Assert.IsType<DB_Order>(okResult.Value);
        Assert.Equal(500, returnedOrder.Total);
    }

    // Stub class for testing
    class StubNotificationService : INotificationService
    {
        public Task NotifyMarketingSystemAsync(int userId, decimal amount) => Task.CompletedTask;
    }
}