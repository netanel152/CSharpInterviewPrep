using CSharpInterviewPrep.Services;
using Xunit;

namespace CSharpInterviewPrep.Tests;

public class StringAlgorithmsTests
{
    [Theory]
    [InlineData("listen", "silent", true)]
    [InlineData("hello", "world", false)]
    [InlineData("hello", "holla", false)]
    [InlineData("triangle", "integral", true)]
    public void AreAnagrams_ShouldReturnExpectedResult(string word1, string word2, bool expected)
    {
        // Act
        bool result = StringAlgorithms.AreAnagrams(word1, word2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("madam", true)]
    [InlineData("racecar", true)]
    [InlineData("hello", false)]
    [InlineData("A man a plan a canal Panama", true)] // Tests TwoPointer logic handling spaces/case
    public void IsPalindromeTwoPointer_ShouldReturnExpectedResult(string input, bool expected)
    {
        bool result = StringAlgorithms.IsPalindromeTwoPointer(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CountCharacters_ShouldCountCorrectly()
    {
        var input = "Hello";
        var result = StringAlgorithms.CountCharacters(input);
        
        Assert.True(result.ContainsKey('h'));
        Assert.Equal(1, result['h']);
        Assert.True(result.ContainsKey('l'));
        Assert.Equal(2, result['l']);
    }

    [Fact]
    public void GetMostFrequentChar_ShouldReturnMostFrequent()
    {
        var input = "banana";
        var result = StringAlgorithms.GetMostFrequentChar(input);
        Assert.Equal('a', result);
    }
}