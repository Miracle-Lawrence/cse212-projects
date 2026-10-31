public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.
    /// For example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.
    /// </summary>
    /// <returns>Array of doubles that are the multiples of the supplied number.</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // PLAN:
        // 1. Create a new double array with the requested length.
        // 2. Loop through each index of the array, starting at index 0.
        // 3. Calculate the multiple by multiplying number by the index plus one.
        // 4. Store the calculated multiple at the current index.
        // 5. Continue until every position in the array has been filled.
        // 6. Return the completed array.

        // IMPLEMENTATION:
        double[] multiples = new double[length];

        for (int i = 0; i < length; i++)
        {
            multiples[i] = number * (i + 1);
        }

        return multiples;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.
    /// For example, rotating {1, 2, 3, 4, 5, 6, 7, 8, 9} by 3
    /// results in {7, 8, 9, 1, 2, 3, 4, 5, 6}.
    /// The value of amount will be in the range of 1 to data.Count, inclusive.
    /// This function modifies the existing list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // PLAN:
        // 1. Calculate the starting index of the final 'amount' elements.
        // 2. Copy those final elements into a temporary list using GetRange().
        // 3. Remove those elements from the original list using RemoveRange().
        // 4. Insert the saved elements at the beginning using InsertRange().
        // 5. The original list will now contain the elements rotated to the right.

        // IMPLEMENTATION:
        int startIndex = data.Count - amount;

        List<int> lastElements = data.GetRange(startIndex, amount);

        data.RemoveRange(startIndex, amount);

        data.InsertRange(0, lastElements);
    }
}
    
