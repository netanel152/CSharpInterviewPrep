using CSharpInterviewPrep.Exercises;
using Xunit;

namespace CSharpInterviewPrep.Tests;

public class ProblemSolvingTests
{
    [Fact]
    public void AreAnagrams_ShouldReturnTrue_ForValidAnagrams()
    {
        // Arrange
        string word1 = "listen";
        string word2 = "silent";

        // Act
        bool result = B_ProblemSolving.AreAnagrams(word1, word2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreAnagrams_ShouldReturnFalse_ForDifferentLengths()
    {
        // Arrange
        string word1 = "hello";
        string word2 = "world";

        // Act
        bool result = B_ProblemSolving.AreAnagrams(word1, word2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreAnagrams_ShouldReturnFalse_ForSameLengthNonAnagrams()
    {
        // Arrange
        string word1 = "hello";
        string word2 = "holla";

        // Act
        bool result = B_ProblemSolving.AreAnagrams(word1, word2);

        // Assert
        Assert.False(result);
    }
}