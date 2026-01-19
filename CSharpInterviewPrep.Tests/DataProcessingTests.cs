using CSharpInterviewPrep.Services;
using static CSharpInterviewPrep.Models.ExerciseModels;

namespace CSharpInterviewPrep.Tests;

public class DataProcessingTests
{
    [Fact]
    public void GetTopStudents_ShouldReturnTop3()
    {
        var students = new List<Student>
        {
            new() { Name = "A", Grade = 50 },
            new() { Name = "B", Grade = 90 },
            new() { Name = "C", Grade = 80 },
            new() { Name = "D", Grade = 95 },
            new() { Name = "E", Grade = 60 }
        };

        var result = DataProcessingService.GetTopStudents(students);
        
        Assert.Equal(3, result.Count);
        Assert.Equal("D", result[0].Name); // 95
        Assert.Equal("B", result[1].Name); // 90
        Assert.Equal("C", result[2].Name); // 80
    }

    [Fact]
    public void FilterAndDoubleEvens_ShouldWorkCorrectly()
    {
        var input = new List<int> { 1, 2, 3, 4, 5 };
        var result = DataProcessingService.FilterAndDoubleEvens(input);
        
        Assert.Equal(new List<int> { 4, 8 }, result);
    }
}