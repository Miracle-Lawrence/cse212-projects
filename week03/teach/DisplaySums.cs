public static class DisplaySums {
    public static void Run() {
        DisplaySumPairs([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);

        Console.WriteLine("------------");
        DisplaySumPairs([-20, -15, -10, -5, 0, 5, 10, 15, 20]);

        Console.WriteLine("------------");
        DisplaySumPairs([5, 11, 2, -4, 6, 8, -1]);
    }

    /// <summary>
    /// Display pairs of numbers that sum to 10 using a set in O(n) time.
    /// We assume there are no duplicate numbers in the list.
    /// </summary>
    private static void DisplaySumPairs(int[] numbers) {
        var previousNumbers = new HashSet<int>();

        foreach (var number in numbers) {
            var requiredNumber = 10 - number;

            if (previousNumbers.Contains(requiredNumber)) {
                Console.WriteLine($"{number} {requiredNumber}");
            }

            previousNumbers.Add(number);
        }
    }
}
