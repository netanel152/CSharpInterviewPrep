using System.Reflection;

namespace CSharpInterviewPrep.Exercises;

public static class I_CriticalConcepts
{
    // ==========================================
    // 1. Yield Return (Lazy Evaluation)
    // ==========================================
    // תרחיש: יש לנו לוג עצום (מיליון שורות). אנחנו לא רוצים לטעון את כולו לזיכרון.
    // Yield Return מאפשר לנו "להזרים" את המידע שורה אחרי שורה.

    public static IEnumerable<int> GetLargeDataset_Lazy()
    {
        Console.WriteLine("   -> Generator started...");
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"   -> Yielding item {i}...");
            yield return i; // המערכת עוצרת כאן ומחזירה ערך, וממשיכה מכאן בפעם הבאה!
        }
        Console.WriteLine("   -> Generator finished.");
    }

    public static List<int> GetLargeDataset_Eager()
    {
        Console.WriteLine("   -> Eager List creation started...");
        var list = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"   -> Adding item {i} to list...");
            list.Add(i);
        }
        Console.WriteLine("   -> Eager List creation finished.");
        return list;
    }

    // ==========================================
    // 2. Reflection & Custom Attributes
    // ==========================================
    // תרחיש: מערכת מרקטינג שצריכה להסתיר מידע רגיש (PII) באופן אוטומטי בהדפסה.

    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveDataAttribute : Attribute { }

    public class UserProfile
    {
        public required string Username { get; set; }

        [SensitiveData] // סימנו את השדה הזה כרגיש
        public required string Email { get; set; }

        [SensitiveData]
        public required string PhoneNumber { get; set; }

        public required string Role { get; set; }
    }

    public static void PrintSafe(object obj)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        Console.WriteLine($"Printing safe data for: {type.Name}");
        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj)?.ToString();

            // בדיקה באמצעות Reflection האם יש Attribute על המאפיין
            if (Attribute.IsDefined(prop, typeof(SensitiveDataAttribute)))
            {
                value = "*** REDACTED ***";
            }

            Console.WriteLine($" - {prop.Name}: {value}");
        }
    }

    // ==========================================
    // 3. IDisposable Pattern (Correct Implementation)
    // ==========================================
    // תרחיש: מחלקה שמחזיקה משאב חיצוני (כמו קובץ פתוח או חיבור רשת) וחייבת לשחרר אותו.

    public class DatabaseConnectionHandler : IDisposable
    {
        private bool _disposed = false;
        public string ConnectionName { get; }

        public DatabaseConnectionHandler(string name)
        {
            ConnectionName = name;
            Console.WriteLine($"[Resource] Connection '{name}' opened.");
        }

        public void ExecuteQuery()
        {
            if (_disposed) throw new ObjectDisposedException(ConnectionName);
            Console.WriteLine($"[Resource] Executing query on '{ConnectionName}'...");
        }

        // 1. Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // אומר ל-GC: "אני כבר ניקיתי, אל תריץ את ה-Finalizer"
        }

        // 2. Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Free managed resources here (Example: close Stream, clear List)
                Console.WriteLine($"[Resource] Connection '{ConnectionName}' closed cleanly (Managed).");
            }

            // Free unmanaged resources here (if any)
            _disposed = true;
        }

        // 3. Finalizer (Destructor) - רק למקרה ששכחו לעשות Dispose
        ~DatabaseConnectionHandler()
        {
            Dispose(false);
            Console.WriteLine($"[Resource] Connection '{ConnectionName}' finalized by GC (Safety Net).");
        }
    }

    public static void Run()
    {
        Console.WriteLine("--- Section 9: Critical C# Concepts ---\n");

        // --- Demo 1: Yield Return ---
        Console.WriteLine("1. Testing Lazy Evaluation (Yield Return):");
        // שימו לב: הלולאה מבקשת את המספר, ורק אז הגנרטור מייצר אותו!
        foreach (var item in GetLargeDataset_Lazy())
        {
            Console.WriteLine($"   Consumer received: {item}");
            if (item == 3)
            {
                Console.WriteLine("   Consumer Stopping early! (Items 4 and 5 will NEVER be generated)");
                break;
            }
        }
        Console.WriteLine("\nvs Eager Evaluation (List):");
        GetLargeDataset_Eager(); // הכל נוצר מראש

        Console.WriteLine("\n--------------------------------------\n");

        // --- Demo 2: Reflection ---
        Console.WriteLine("2. Testing Reflection & Attributes:");
        var user = new UserProfile
        {
            Username = "john_doe",
            Email = "john@company.com",
            PhoneNumber = "555-1234",
            Role = "Admin"
        };
        PrintSafe(user);

        Console.WriteLine("\n--------------------------------------\n");

        // --- Demo 3: IDisposable ---
        Console.WriteLine("3. Testing IDisposable:");

        // השימוש הנכון: using statement שמבטיח קריאה ל-Dispose בסוף הבלוק
        using (var db = new DatabaseConnectionHandler("ProductionDB"))
        {
            db.ExecuteQuery();
        } // Dispose נקרא אוטומטית כאן

        Console.WriteLine("End of scope.");
    }
}