namespace Savidiy.Utils
{
    public static class ArrayExtensions
    {
        public static int[,] CloneArray(this int[,] fromArray)
        {
            int countA = fromArray.GetLength(0);
            int countB = fromArray.GetLength(1);
            var ints = new int [countA, countB];

            for (int i = 0; i < countA; i++)
            for (int j = 0; j < countB; j++)
                ints[i, j] = fromArray[i, j];

            return ints;
        }
    }
}