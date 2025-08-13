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

        public class User
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string Role { get; set; }
        }

        public class UserProfile
        {
            public string? Name { get; set; }
            public List<string>? Permissions { get; set; }
            public List<string>? RecentActivity { get; set; }
        }

        public class Change
        {
            public string? PropertyName { get; set; }
            public string? OldValue { get; set; }
            public string? NewValue { get; set; }
        }

        public class Fruit
        {
            public required string Name { get; set; }
            public required string Color { get; set; }
            public decimal Price { get; set; }
        }
    }
}
