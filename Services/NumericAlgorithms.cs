namespace CSharpInterviewPrep.Services;

/// <summary>
/// Provides algorithms for numeric and math operations.
/// </summary>
public static class NumericAlgorithms
{
    /// <summary>
    /// Generates the Fibonacci sequence up to the specified number of terms.
    /// </summary>
    public static IEnumerable<int> GenerateFibonacciSeries(int terms)
    {
        int a = 0, b = 1;
        for (int i = 0; i < terms; i++)
        {
            yield return a;
            int next = a + b;
            a = b;
            b = next;
        }
    }

    /// <summary>
    /// Generates a FizzBuzz sequence up to the limit.
    /// </summary>
    public static IEnumerable<string> GenerateFizzBuzz(int limit = 100)
    {
        for (int n = 1; n <= limit; n++)
        {
            if (n % 15 == 0) yield return "FizzBuzz";
            else if (n % 3 == 0) yield return "Fizz";
            else if (n % 5 == 0) yield return "Buzz";
            else yield return n.ToString();
        }
    }

    /// <summary>
    /// Multiplies numbers and returns a formatted table string.
    /// </summary>
    public static List<string> GetMultiplicationTable(int number, int limit = 10)
    {
        var table = new List<string>();
        for (int i = 1; i <= limit; i++)
        {
            table.Add($"{number} x {i} = {number * i}");
        }
        return table;
    }
}