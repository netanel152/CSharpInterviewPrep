using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Exercises;

//Strategy Factory Pattern
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

//Singleton Pattern
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

// איך משתמשים בו מכל מקום במערכת:
//var connectionString = ConfigurationManager.Instance.GetSetting("DatabaseConnectionString");



//Adapter Pattern
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

// איך משתמשים בו:
// ILogger logger = new LoggerAdapter();
// logger.Log("This message will be written by the legacy logger.");


// ה-"נושא" (המוצר בחנות)
public class ProductEvent
{
    public event EventHandler<PriceChangedEventArgs> PriceChanged;

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
    public void OnPriceChanged(object sender, PriceChangedEventArgs e)
    {
        Console.WriteLine($"Price changed! Old: {e.OldPrice:C}, New: {e.NewPrice:C}. I should consider buying!");
    }
}
