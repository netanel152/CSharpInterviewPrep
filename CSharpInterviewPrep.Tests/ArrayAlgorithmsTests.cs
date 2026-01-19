using CSharpInterviewPrep.Services;

namespace CSharpInterviewPrep.Tests;

public class ArrayAlgorithmsTests
{
    [Fact]
    public void FindFirstDuplicate_ShouldReturnDuplicate()
    {
        int[] input = { 2, 5, 1, 2, 3 };
        var result = ArrayAlgorithms.FindFirstDuplicate(input);
        Assert.Equal(2, result);
    }

    [Fact]
    public void FindFirstDuplicate_ShouldReturnNull_WhenNoDuplicates()
    {
        int[] input = { 1, 2, 3, 4, 5 };
        var result = ArrayAlgorithms.FindFirstDuplicate(input);
        Assert.Null(result);
    }

    [Fact]
    public void MergeSortedArrays_ShouldMergeCorrectly()
    {
        int[] arr1 = { 1, 3, 5 };
        int[] arr2 = { 2, 4, 6 };
        var result = ArrayAlgorithms.MergeSortedArrays(arr1, arr2);
        
        Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result);
    }

    [Fact]
    public void RotateArray_ShouldRotateCorrectly()
    {
        int[] arr = { 1, 2, 3, 4, 5 };
        int k = 2;
        var result = ArrayAlgorithms.RotateArray(arr, k);
        Assert.Equal(new[] { 4, 5, 1, 2, 3 }, result);
    }

    [Fact]
    public void HasPairWithSum_ShouldReturnTrue_WhenPairExists()
    {
        int[] arr = { 1, 2, 3, 9 }; // Sum 8 not possible, sum 5 (2+3) possible
        Assert.True(ArrayAlgorithms.HasPairWithSum(arr, 5));
        Assert.False(ArrayAlgorithms.HasPairWithSum(arr, 8));
    }
}