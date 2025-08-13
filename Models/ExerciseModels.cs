namespace CSharpInterviewPrep.Models
{
    public class ExerciseModels
    {
        public class Product
        {
            public int Id { get; set; }
            public required string Name { get; set; }
        }
        public class Order
        {
            public int ProductId { get; set; }
            public required string Category { get; set; }
            public decimal Price { get; set; }
            public decimal TotalWeight { get; internal set; }
        }
        public class Student
        {
            public required string Name { get; set; }
            public int Grade { get; set; }
        }

        public class UserProfile
        {
            public string? Name { get; set; }
            public List<string>? Permissions { get; set; }
            public List<string>? RecentActivity { get; set; }
        }
    }
}
