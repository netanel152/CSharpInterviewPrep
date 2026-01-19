namespace CSharpInterviewPrep.Exercises;

public static class N_ModernLinq
{
    // Modern LINQ Features (.NET 6, 7, 8, 9)
    // Demonstrating: CountBy, AggregateBy, Chunk, MaxBy, DistinctBy
    
    public static void Run()
    {
        Console.WriteLine("--- Section 14: Modern LINQ Features (.NET 6-9) ---");

        var employees = new List<Employee>
        {
            new() { Id = 1, Name = "Alice", Department = "HR", Salary = 50000 },
            new() { Id = 2, Name = "Bob", Department = "IT", Salary = 80000 },
            new() { Id = 3, Name = "Charlie", Department = "HR", Salary = 55000 },
            new() { Id = 4, Name = "Dave", Department = "IT", Salary = 85000 },
            new() { Id = 5, Name = "Eve", Department = "Sales", Salary = 60000 },
            new() { Id = 6, Name = "Frank", Department = "IT", Salary = 90000 }
        };

        // 1. MaxBy / MinBy (.NET 6)
        // Returns the element that has the max/min value for the specified key.
        var highestPaid = employees.MaxBy(e => e.Salary);
        var lowestPaid = employees.MinBy(e => e.Salary);
        Console.WriteLine($"1. MaxBy: Highest paid is {highestPaid?.Name} ({highestPaid?.Salary:C})");

        // 2. DistinctBy (.NET 6)
        // Returns distinct elements based on a specific key selector.
        var distinctDepartments = employees.DistinctBy(e => e.Department).Select(e => e.Department);
        Console.WriteLine($"2. DistinctBy: Departments found: {string.Join(", ", distinctDepartments)}");

        // 3. Chunk (.NET 6)
        // Splits the collection into chunks of a specified size. Great for batch processing.
        var chunks = employees.Chunk(2);
        Console.WriteLine("3. Chunk (Batch of 2):");
        int chunkId = 1;
        foreach (var chunk in chunks)
        {
            Console.WriteLine($"   Batch {chunkId++}: {string.Join(", ", chunk.Select(e => e.Name))}");
        }

        // 4. CountBy (.NET 9)
        // Quickly counts the occurrences of keys. 
        // Replaces: GroupBy(x => x.Key).Select(g => new { Key = g.Key, Count = g.Count() })
        Console.WriteLine("4. CountBy (.NET 9): Employees per Department");
        foreach (var entry in employees.CountBy(e => e.Department))
        {
            Console.WriteLine($"   {entry.Key}: {entry.Value}");
        }

        // 5. AggregateBy (.NET 9)
        // Aggregates values based on a key.
        // Replaces complex GroupBy + Sum/Aggregate logic.
        Console.WriteLine("5. AggregateBy (.NET 9): Total Salary per Department");
        
        // KeySelector, Seed, Func<Accumulator, Element, Accumulator>
        var salaryByDept = employees.AggregateBy(
            keySelector: e => e.Department,
            seed: 0m,
            func: (total, emp) => total + emp.Salary
        );

        foreach (var entry in salaryByDept)
        {
            Console.WriteLine($"   {entry.Key}: {entry.Value:C}");
        }
        
        // 6. Index (.NET 9)
        // Provides the index along with the element in a tuple.
        Console.WriteLine("6. Index (.NET 9): Enumerating with index");
        foreach (var (index, employee) in employees.Index())
        {
             Console.WriteLine($"   Index {index}: {employee.Name}");
        }

        Console.WriteLine("---------------------------------------------------\n");
    }

    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Department { get; set; }
        public decimal Salary { get; set; }
    }
}
