using CSharpInterviewPrep.Services;

namespace CSharpInterviewPrep.Exercises;

public static class B_ProblemSolving
{
    public static void Run()
    {
        Console.WriteLine("--- Section 2: Problem Solving ---");
        bool isAnagram = StringAlgorithms.AreAnagrams("listen", "silent");
        Console.WriteLine($"AreAnagrams('listen', 'silent'): {isAnagram}");

        int? duplicate = ArrayAlgorithms.FindFirstDuplicate(new[] { 2, 5, 1, 2, 3 });
        Console.WriteLine($"FindFirstDuplicate: Found {duplicate}");

        string text = "This is a test. This test is only a test.";
        var topWords = StringAlgorithms.GetTop10CommonWords(text);
        Console.WriteLine("GetTop10CommonWords: Top words are:");
        foreach (var word in topWords)
        {
            Console.WriteLine($"- {word}");
        }

        int[] originalArray = { 1, 2, 3, 4, 5 };
        int[] rotated = ArrayAlgorithms.RotateArray(originalArray, 2);
        Console.WriteLine($"RotateArray: Rotated array is {string.Join(", ", rotated)}");

        Console.WriteLine("--- Demonstrating HasPairWithSum ---");
        int[] numbers = { 1, 2, 5, 4, 3 };
        int targetSum = 6;
        bool hasPair = ArrayAlgorithms.HasPairWithSum(numbers, targetSum);
        Console.WriteLine($"HasPairWithSum: Pair with sum {targetSum} exists: {hasPair}");

        Console.WriteLine("--- Demonstrating FindSecondLargestNumber ---");
        List<int> list = [3, 1, 4, 4, 5, 2];
        int? secondLargest = ArrayAlgorithms.FindSecondLargestNumber(list);
        Console.WriteLine($"Second largest number in {string.Join(", ", list)} is: {secondLargest}");
        Console.WriteLine("---------------------------------\n");
    }
}