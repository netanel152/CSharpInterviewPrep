using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInterviewPrep.Exercises
{
    public class F_HighPerformance
    {
        // --- Scenario 1: Parsing text without Allocations (Span<T>) ---
        // בחברות Big Data, חיתוך סטרינגים (Substring) הוא יקר כי הוא מייצר סטרינג חדש בזיכרון.
        // Span מאפשר להסתכל על חלק מהזיכרון בלי להעתיק אותו.

        public static void CompareStringVsSpan()
        {
            string data = "User:JohnDoe|Role:Admin|Active:True|Region:IL";

            // ❌ הדרך הישנה והבזבזנית (מייצרת 4 מחרוזות חדשות בזיכרון)
            string roleOld = data.Split('|')[1].Split(':')[1];

            // ✅ הדרך היעילה (0 הקצאות זיכרון - Zero Allocation)
            ReadOnlySpan<char> span = data.AsSpan();
            int roleIndex = span.IndexOf("Role:");
            ReadOnlySpan<char> afterRole = span.Slice(roleIndex + 5); // דלג על "Role:"
            int separatorIndex = afterRole.IndexOf('|');
            ReadOnlySpan<char> roleVal = afterRole.Slice(0, separatorIndex);

            Console.WriteLine($"Extracted Role (Span): {roleVal.ToString()}");
            // הערה: בחיים האמיתיים נשתמש ב-roleVal ללוגיקה בלי להפוך ל-ToString אם אפשר.
        }

        // --- Scenario 2: High Performance Structs (ref struct) ---
        // שימוש ב-Struct במקום Class למניעת לחץ על ה-GC
        public readonly struct Point3D
        {
            public readonly double X, Y, Z;
            public Point3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);
        }

        public static void ProcessPoints()
        {
            // הקצאה על ה-Stack (מהיר מאוד) במקום על ה-Heap
            Span<Point3D> points = stackalloc Point3D[100];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point3D(i, i * 2, i + 5);
            }
            Console.WriteLine($"Processed {points.Length} points on the Stack instantly.");
        }

        public static void Run()
        {
            Console.WriteLine("--- Section 6: High Performance ---");
            CompareStringVsSpan();
            ProcessPoints();
            Console.WriteLine("---------------------------------\n");
        }
    }
}
