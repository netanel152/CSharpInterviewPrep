using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInterviewPrep.Models
{
    public class ExerciseModels
    {
        public class Product { public int Id { get; set; } public required string Name { get; set; } }
        public class Order { public required string Category { get; set; } public decimal Price { get; set; } }
        public class Student { public required string Name { get; set; } public int Grade { get; set; } }
        public class Item { public required string Id { get; set; } public decimal Price { get; set; } }

        public class FileMetadata
        {
            public required string FilePath { get; set; }
            public required string FileHash { get; set; }
        }

        public class Document
        {
            public int Id { get; set; }
            public required string Content { get; set; }
            public DateTime CreatedAt { get; set; }
            public Dictionary<string, string> Fields { get; set; } = new();
        }

        public class SearchResult
        {
            public required string Title { get; set; }
            public List<string> AllowedGroups { get; set; } = new();
        }

        public class User
        {
            public List<string> MemberOfGroups { get; set; } = new();
        }
    }
}
