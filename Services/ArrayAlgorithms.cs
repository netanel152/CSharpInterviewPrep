namespace CSharpInterviewPrep.Services;

/// <summary>
/// Provides algorithms for array and collection manipulations.
/// </summary>
public static class ArrayAlgorithms
{
    /// <summary>
    /// Finds the first duplicate number in an array.
    /// </summary>
    public static int? FindFirstDuplicate(int[] numbers)
    {
        var seenNumbers = new HashSet<int>();
        foreach (var number in numbers)
        {
            if (!seenNumbers.Add(number))
            {
                return number;
            }
        }
        return null;
    }

    /// <summary>
    /// Rotates an array to the right by k steps.
    /// </summary>
    public static int[] RotateArray(int[] arr, int k)
    {
        if (arr == null || arr.Length == 0)
        {
            throw new ArgumentException("Array cannot be empty.");
        }
        if (k < 0)
        {
            throw new ArgumentException("Rotation count cannot be negative.");
        }

        k = k % arr.Length;
        if (k == 0) return (int[])arr.Clone();

        int[] rotatedArray = new int[arr.Length];
        Array.Copy(arr, arr.Length - k, rotatedArray, 0, k);
        Array.Copy(arr, 0, rotatedArray, k, arr.Length - k);
        return rotatedArray;
    }

    /// <summary>
    /// Merges two sorted arrays into a single sorted array.
    /// </summary>
    public static int[] MergeSortedArrays(int[] arr1, int[] arr2)
    {
        int[] mergeArr = new int[arr1.Length + arr2.Length];
        int i = 0, j = 0, m = 0;

        while (i < arr1.Length && j < arr2.Length)
        {
            if (arr1[i] < arr2[j])
            {
                mergeArr[m++] = arr1[i++];
            }
            else
            {
                mergeArr[m++] = arr2[j++];
            }
        }

        while (i < arr1.Length)
        {
            mergeArr[m++] = arr1[i++];
        }
        while (j < arr2.Length)
        {
            mergeArr[m++] = arr2[j++];
        }

        return mergeArr;
    }

    /// <summary>
    /// Checks if there exists a pair of numbers in the array that sums up to the target.
    /// </summary>
    public static bool HasPairWithSum(int[] numbers, int target)
    {
        if (numbers == null || numbers.Length < 2)
        {
            return false;
        }

        // We sort the array first to use the two-pointer technique.
        // Note: sorting modifies the array or requires a copy. Here we assume modification is allowed
        // or the caller should pass a copy.
        Array.Sort(numbers);
        int left = 0, right = numbers.Length - 1;
        while (left < right)
        {
            int sum = numbers[left] + numbers[right];

            if (sum == target)
                return true;

            if (sum < target)
            {
                left++;
            }
            else
            {
                right--;
            }
        }
        return false;
    }

    /// <summary>
    /// Finds the second largest number in a list of integers.
    /// </summary>
    public static int FindSecondLargestNumber(List<int> numbers)
    {
        if (numbers == null || numbers.Count < 2)
            throw new ArgumentException("List must contain at least two numbers.");

        int largest = int.MinValue;
        int second = int.MinValue;

        foreach (var number in numbers)
        {
            if (number > largest)
            {
                second = largest;
                largest = number;
            }
            else if (number > second && number < largest)
            {
                second = number;
            }
        }
        if (second == int.MinValue)
        {
             // This might happen if all numbers are the same, e.g., [5, 5, 5]
             // The original logic threw an exception here, which we preserve.
            throw new InvalidOperationException("There is no second largest number in the list.");
        }
        return second;
    }
}