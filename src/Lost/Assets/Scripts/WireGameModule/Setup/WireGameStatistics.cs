using System.Collections.Generic;
using Savidiy.Utils;
using WireGameModule.Model;

namespace WireGameModule.Setup
{
    public class WireGameStatistics
    {
        private readonly IndexCombinator _indexCombinator = new();

        public int UpdateStatistics(List<string> statistics, List<PointPair> startConnections, int[,] connectsValue)
        {
            statistics.Clear();

            if (TryCalcSum(startConnections, connectsValue, out var startSum))
                statistics.Add($"Start sum = '{startSum}'");
            else
                statistics.Add("Has error in start connection");


            List<List<PointPair>> potentialConnections = GetPotentialConnections(startConnections.Count, connectsValue);

            var results = new List<Result>();
            foreach (List<PointPair> potentialConnection in potentialConnections)
            {
                TryCalcSum(potentialConnection, connectsValue, out int sum);
                results.Add(new Result(sum, potentialConnection));
            }

            results.Sort((a, b) => b.Sum.CompareTo(a.Sum));

            statistics.Add(CommonStatistics(results));

            foreach (Result result in results)
            {
                statistics.Add(PrintResult(result));
            }

            return results[0].Sum;
        }

        private string CommonStatistics(List<Result> results)
        {
            Dictionary<int, int> stat = new();
            foreach (Result result in results)
            {
                int key = result.Sum;
                if (!stat.ContainsKey(key))
                {
                    stat[key] = 0;
                }

                stat[key]++;
            }

            string text = "Stat: ";
            foreach ((int key, int value) in stat)
            {
                text += $"{key}x{value}, ";
            }

            return text;
        }

        private List<List<PointPair>> GetPotentialConnections(int count, int[,] connectsValue)
        {
            int lengthA = connectsValue.GetLength(0);
            int lengthB = connectsValue.GetLength(1);

            List<IReadOnlyList<int>> combinationsA = GetCombinationsIndexes(count, lengthA);
            List<IReadOnlyList<int>> combinationsB = GetCombinationsIndexes(count, lengthB);

            var result = new List<List<PointPair>>();

            foreach (IReadOnlyList<int> indexesA in combinationsA)
            {
                foreach (IReadOnlyList<int> indexesB in combinationsB)
                {
                    List<PointPair> cross = new List<PointPair>();
                    for (int i = 0; i < count; i++)
                    {
                        cross.Add(new PointPair(indexesA[i], indexesB[i]));
                    }

                    if (HasNotSameCross(result, cross))
                        result.Add(cross);
                }
            }

            return result;
        }

        private bool HasNotSameCross(List<List<PointPair>> result, List<PointPair> cross)
        {
            foreach (List<PointPair> existedPairs in result)
            {
                int samePairsCount = existedPairs.Count;

                foreach (PointPair existedPair in existedPairs)
                {
                    foreach (PointPair newPair in cross)
                    {
                        if (newPair.EqualsPair(existedPair))
                            samePairsCount--;
                    }
                }

                if (samePairsCount == 0)
                    return false;
            }

            return true;
        }

        private List<IReadOnlyList<int>> GetCombinationsIndexes(int targetCount, int allCount)
        {
            return _indexCombinator.GetIndexCombinations(targetCount, allCount);
        }

        private string PrintResult(Result result)
        {
            string text = $"{result.Sum}: ";
            foreach (PointPair pointPair in result.Connections)
            {
                text += $"A{pointPair.IndexA}-B{pointPair.IndexB} ";
            }

            return text;
        }

        private bool TryCalcSum(List<PointPair> connections, int[,] values, out int sum)
        {
            sum = 0;
            int lengthA = values.GetLength(0);
            int lengthB = values.GetLength(1);
            foreach (PointPair pair in connections)
            {
                int indexA = pair.IndexA;
                int indexB = pair.IndexB;

                if (indexA >= lengthA || indexB >= lengthB)
                    return false;

                sum += values[indexA, indexB];
            }

            return true;
        }

        class Result
        {
            public int Sum { get; }
            public List<PointPair> Connections { get; }

            public Result(int sum, List<PointPair> connections)
            {
                Sum = sum;
                Connections = connections;
            }
        }
    }
}