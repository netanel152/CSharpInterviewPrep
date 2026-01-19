using System.Diagnostics;

namespace CSharpInterviewPrep.Exercises;

// Middleware allows you to intercept requests BEFORE they reach the controller
// and AFTER the controller finishes.
public static class H_Middleware
{
    // A delegate representing the next middleware in the pipeline
    public delegate Task RequestDelegate(string context);

    // --- 1. Global Error Handling Middleware ---
    // תפקידו: לתפוס כל שגיאה שנזרקת במערכת ולמנוע קריסה של השרת
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(string context)
        {
            try
            {
                // מעביר את הבקשה הלאה ל-Middleware הבא
                await _next(context);
            }
            catch (Exception ex)
            {
                // אם משהו התפוצץ במעמקי המערכת - אנחנו נתפוס את זה כאן
                Console.WriteLine($"[Global Error Handler] 🛑 Caught exception: {ex.Message}");
                Console.WriteLine($"[Global Error Handler] Sending '500 Internal Server Error' to client.");
            }
        }
    }

    // --- 2. Performance Logging Middleware ---
    // תפקידו: למדוד בדיוק כמה זמן לוקח לטפל בבקשה
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(string context)
        {
            var watch = Stopwatch.StartNew();
            Console.WriteLine($"[Performance] Started processing: {context}");

            // מעביר את השליטה הלאה ללוגיקה העסקית
            await _next(context);

            // הקוד כאן רץ *אחרי* שהלוגיקה הסתיימה
            watch.Stop();
            Console.WriteLine($"[Performance] Finished {context}. Time: {watch.ElapsedMilliseconds} ms");

            if (watch.ElapsedMilliseconds > 500)
            {
                Console.WriteLine($"[Performance] WARNING: Request took too long!");
            }
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("--- Section 8: Middleware Pipeline Simulation ---");

        // The "Business Logic" (Simulating a Controller Endpoint)
        // זה הקוד שרץ בסוף השרשרת
        RequestDelegate finalEndpoint = async (ctx) =>
        {
            Console.WriteLine($"   --> Executing Business Logic for {ctx}...");

            // סימולציה של עבודה
            await Task.Delay(100);

            // סימולציה של שגיאה במקרה מסוים
            if (ctx.Contains("crash"))
            {
                throw new InvalidOperationException("Something went wrong inside the Database!");
            }

            // סימולציה של איטיות
            if (ctx.Contains("slow"))
            {
                await Task.Delay(600);
            }
        };

        // Building the Pipeline: ErrorHandler -> Timing -> Logic
        // אנחנו בונים את ה"בובות בבושקה" מבפנים החוצה

        // 1. עוטפים את הלוגיקה עם טיימר
        var timingMiddleware = new RequestTimingMiddleware(finalEndpoint);
        RequestDelegate timingDelegate = timingMiddleware.InvokeAsync;

        // 2. עוטפים את הטיימר עם תופס שגיאות
        var errorMiddleware = new ErrorHandlingMiddleware(timingDelegate);

        // Test 1: Successful Request 
        Console.WriteLine("\nTest 1: Normal Request");
        await errorMiddleware.InvokeAsync("/api/products");

        // Test 2: Slow Request (Performance Warning) 
        Console.WriteLine("\nTest 2: Slow Request");
        await errorMiddleware.InvokeAsync("/api/heavy-report (slow)");

        // Test 3: Crashing Request (Error Handling) 
        Console.WriteLine("\nTest 3: Crashing Request");
        await errorMiddleware.InvokeAsync("/api/buggy-feature (crash)");

        Console.WriteLine("--------------------------------------\n");
    }
}