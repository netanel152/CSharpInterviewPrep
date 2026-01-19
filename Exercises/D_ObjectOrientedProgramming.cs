using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

// --- Part 1: Strategy Factory Pattern ---
// 1. הגדרת החוזה (הממשק)
public interface IShippingStrategy
{
    decimal CalculateShippingCost(Order order);
}

// 2. מימושים קונקרטיים לאסטרטגיות
public class WeightShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(Order order) => order.TotalWeight * 1.5m;
}

public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(Order order) => 25.0m; // מחיר קבוע למשלוח מהיר
}

public class FreeShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(Order order) => 0.0m; // חינם
}

public class DistanceShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(Order order)
    {
        const decimal distance = 10.0m; // נניח שהמרחק הוא 10 קילומטרים
        return distance * 2.0m; // מחיר לקילומטר
    }
}

// 3. ה-Factory שיודע ליצור את האסטרטגיה הנכונה
public static class ShippingStrategyFactory
{
    public static IShippingStrategy GetStrategy(string strategyType)
    {
        return strategyType.ToLower() switch
        {
            "weight" => new WeightShippingStrategy(),
            "express" => new ExpressShippingStrategy(),
            "free" => new FreeShippingStrategy(),
            "distance" => new DistanceShippingStrategy(),
            _ => throw new NotSupportedException("This shipping type is not supported.")
        };
    }
}

// --- Part 2: Singleton Pattern ---
public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> lazyInstance =
        new Lazy<ConfigurationManager>(() => new ConfigurationManager());

    public static ConfigurationManager Instance => lazyInstance.Value;

    private Dictionary<string, string> _settings = new();

    // הבנאי הוא פרטי כדי למנוע יצירה מבחוץ
    private ConfigurationManager()
    {
        // טוען הגדרות מקובץ או ממקור אחר
        Console.WriteLine("ConfigurationManager instance created.");
        _settings.Add("DatabaseConnectionString", "server=prod;db=main");
        _settings.Add("ApiEndpoint", "https://api.example.com");
        _settings.Add("CacheTimeout", "300");
        _settings.Add("LoggingLevel", "Info");
        _settings.Add("FeatureToggle_NewFeature", "true");
        _settings.Add("FeatureToggle_OldFeature", "false");
        _settings.Add("FeatureToggle_ExperimentalFeature", "true");
        _settings.Add("FeatureToggle_AnotherFeature", "false");
    }

    public string GetSetting(string key)
    {
        return _settings.GetValueOrDefault(key, "default_value");
    }
}

// --- Part 3: Adapter Pattern ---
// הממשק הישן והלא תואם (למשל, מספרייה חיצונית)
public class LegacyLogger
{
    public void WriteEntry(string message)
    {
        Console.WriteLine($"Legacy Log: {message}");
    }
}

// הממשק המודרני והסטנדרטי במערכת שלנו
public interface ILogger
{
    void Log(string message);
}

// המתאם! הוא מממש את הממשק שלנו, אבל "עוטף" את הלוגר הישן
public class LoggerAdapter : ILogger
{
    private readonly LegacyLogger _legacyLogger = new LegacyLogger();

    public void Log(string message)
    {
        // כאן מתבצע התרגום: קריאה למתודה המודרנית מפעילה את המתודה הישנה
        _legacyLogger.WriteEntry(message);
    }
}

// --- Part 4: Observer Pattern (Events & Delegates) ---
// ה-"נושא" (המוצר בחנות)
public class ProductEvent
{
    public event EventHandler<PriceChangedEventArgs>? PriceChanged;

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value) return;
            var oldPrice = _price;
            _price = value;
            // "יורה" את האירוע לכל מי שמאזין
            PriceChanged?.Invoke(this, new PriceChangedEventArgs { OldPrice = oldPrice, NewPrice = _price });
        }
    }
}

public class PriceChangedEventArgs : EventArgs
{
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}

// ה-"צופה" (הלקוח שמעוניין במוצר)
public class Customer
{
    public void OnPriceChanged(object? sender, PriceChangedEventArgs e)
    {
        Console.WriteLine($"Price changed! Old: {e.OldPrice:C}, New: {e.NewPrice:C}. I should consider buying!");
    }
}

// --- Part 5: Struct vs Class ---
public readonly struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    // פעולה שמחזירה אובייקט Money חדש
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException("Cannot add money of different currencies.");
        }
        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}

// --- Part 6: Auction System (Events) ---
public class NewBidEventArgs : EventArgs
{
    public decimal BidAmount { get; set; }
    public required string BidderName { get; set; }
}

public class AuctionItem
{
    // הגדרת האירוע. הוא משתמש ב-delegate הגנרי EventHandler
    public event EventHandler<NewBidEventArgs>? NewBidPlaced;

    public string Name { get; }
    public decimal CurrentHighestBid { get; private set; } = 0;

    public AuctionItem(string name)
    {
        Name = name;
    }

    public void PlaceBid(string bidderName, decimal amount)
    {
        if (amount > CurrentHighestBid)
        {
            Console.WriteLine($"\n{bidderName} placed a new high bid of {amount:C} for {Name}!");
            CurrentHighestBid = amount;
            // "ירי" של האירוע לכל המאזינים
            OnNewBidPlaced(new NewBidEventArgs { BidAmount = amount, BidderName = bidderName });
        }
        else
        {
            Console.WriteLine($"{bidderName}'s bid of {amount:C} is not higher than the current bid.");
        }
    }

    protected virtual void OnNewBidPlaced(NewBidEventArgs e)
    {
        NewBidPlaced?.Invoke(this, e);
    }
}

// 3. ה"צופה" (Observer) - המציע במכירה
public class Bidder
{
    public string Name { get; }

    public Bidder(string name)
    {
        Name = name;
    }

    // זוהי המתודה שתופעל כשהאירוע יקרה
    public void OnNewBidPlacedHandler(object? sender, NewBidEventArgs e)
    {
        // אם אני לא זה שהצעתי את ההצעה, אני מקבל התראה
        if (e.BidderName != Name)
        {
            Console.WriteLine($"  -> {Name} notified: New highest bid of {e.BidAmount:C} was placed by {e.BidderName}.");
        }
    }
}

// --- Part 7: Polymorphism & Interface Injection (Notification System) ---
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

// --- Part 8: Decorator Pattern (Caching) ---
public interface IRepository
{
    Task<string> GetById(int id);
}

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


public static class D_ObjectOrientedProgramming
{
    public static void Run()
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

        // Moved from Advanced Topics
        Console.WriteLine("\n--- Demonstrating Flexible Notification System ---");
        var emailSender = new EmailSender();
        var smsSender = new SmsSender();
        var whatsappSender = new WhatsAppSender();
        var notificationService = new NotificationService(new List<INotificationSender> { emailSender, smsSender, whatsappSender });
        // NOTE: This is async but running synchronously here for simplicity in this void method, 
        // or we could change Run to async Task.
        notificationService.SendAllNotificationsAsync("user123", "Your order has been shipped!").Wait();

        Console.WriteLine("\n--- Demonstrating Caching Decorator Pattern ---");
        var cache = new MemoryCache(new MemoryCacheOptions());
        IRepository repository = new CachingRepository(new SlowRepository(), cache);

        Console.WriteLine("First call (should be slow):");
        var stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        repository.GetById(123).Wait();
        stopwatch.Stop();
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine("\nSecond call (should be fast and from cache):");
        stopwatch.Restart();
        repository.GetById(123).Wait();
        stopwatch.Stop();
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

        Console.WriteLine("---------------------------------\n");
    }
}