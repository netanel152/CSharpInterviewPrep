using System.Diagnostics;

namespace CSharpInterviewPrep.Exercises;

public static class L_MemoryManagement
{
    // 1. Span<T> and StackAlloc (Zero Allocation Slicing)
    // רלוונטי לביצועים גבוהים: מניעת הקצאות זיכרון מיותרות ב-Heap
    // Scenario: Parsing a large string (e.g., Log entry, DNA sequence, CSV line) without creating new substrings.

    public static void SpanDemo()
    {
        Console.WriteLine("1. Span<T> vs Substring (Memory Allocation):");

        string dateString = "2026-01-19"; // YYYY-MM-DD

        // BAD: Creating new strings for each part (Allocates on Heap)
        string yearStr = dateString.Substring(0, 4);
        string monthStr = dateString.Substring(5, 2);
        string dayStr = dateString.Substring(8, 2);
        
        // GOOD: Using Span to "view" the memory without allocation
        ReadOnlySpan<char> dateSpan = dateString.AsSpan();
        ReadOnlySpan<char> yearSpan = dateSpan.Slice(0, 4);
        ReadOnlySpan<char> monthSpan = dateSpan.Slice(5, 2);
        ReadOnlySpan<char> daySpan = dateSpan.Slice(8, 2);

        // Parsing directly from Span (Supported in modern .NET)
        int year = int.Parse(yearSpan);
        int month = int.Parse(monthSpan);
        int day = int.Parse(daySpan);

        Console.WriteLine($"   Parsed Date: {day}/{month}/{year} (Zero String Allocations!)");
    }

    // 2. Struct vs Class (Stack vs Heap) & Boxing
    // Scenario: High-frequency data structures (e.g., Coordinates, Points in a game/map).

    struct PointStruct { public int X, Y; }
    class PointClass { public int X, Y; }

    public static void StackVsHeapDemo()
    {
        Console.WriteLine("\n2. Stack (Struct) vs Heap (Class) Performance:");

        const int iterations = 1_000_000;
        var sw = Stopwatch.StartNew();

        // Heap Allocation
        for (int i = 0; i < iterations; i++)
        {
            var p = new PointClass { X = i, Y = i };
        }
        sw.Stop();
        Console.WriteLine($"   Class (Heap) time: {sw.ElapsedMilliseconds}ms");

        sw.Restart();

        // Stack Allocation
        for (int i = 0; i < iterations; i++)
        {
            var p = new PointStruct { X = i, Y = i };
        }
        sw.Stop();
        Console.WriteLine($"   Struct (Stack) time: {sw.ElapsedMilliseconds}ms (Much faster!)");
    }

    
    // 3. Boxing & Unboxing (Hidden Cost)
    // מלכודת נפוצה: המרת Value Type ל-Reference Type (Object/Interface)

    public static void BoxingDemo()
    {
        Console.WriteLine("\n3. Boxing & Unboxing Costs:");

        int val = 123;
        object boxed = val; // Boxing: Moves 'val' from Stack to Heap! (Expensive)
        int unboxed = (int)boxed; // Unboxing: Extracting back to Stack.

        Console.WriteLine($"   Value: {val}, Boxed: {boxed}, Unboxed: {unboxed}");
        Console.WriteLine("   Rule: Avoid storing structs in List<object> or non-generic interfaces in hot paths.");
    }

    public static void Run()
    {
        Console.WriteLine("--- Section 12: Memory Management & Performance ---");
        SpanDemo();
        StackVsHeapDemo();
        BoxingDemo();
        Console.WriteLine("---------------------------------------------------\n");
    }
}
