using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using WireGameModule.Infrastructure;

namespace WireGameModule.Model
{
    public class WireGameLevelData
    {
        private readonly int[,] _connectsValue;
        private readonly ReactiveProperty<int> _currentSum = new();

        public Sprite BackSprite { get; }
        public List<Vector3> PointsA { get; }
        public List<Vector3> PointsB { get; }
        public List<PointPair> Connections { get; }
        public int TargetSum { get; }
        public IReadOnlyReactiveProperty<int> CurrentSum => _currentSum;

        public WireGameLevelData(WireGameLevel level)
        {
            Connections = level.StartConnections.ToList();

            PointsA = level.PointsA;
            PointsB = level.PointsB;
            BackSprite = level.BackSprite;
            TargetSum = level.TargetSum;
            _connectsValue = level.ConnectsValue;

            CalcCurrentSum();
        }

        private void CalcCurrentSum()
        {
            int sum = 0;
            foreach (PointPair pointPair in Connections)
            {
                sum += _connectsValue[pointPair.IndexA, pointPair.IndexB];
            }

            _currentSum.Value = sum;
        }

        public void SwapConnections(EPointGroup group, int previousIndex, int newIndex)
        {
            PointPair firstPair = null;
            PointPair secondPair = null;

            if (group == EPointGroup.A)
            {
                foreach (PointPair pointPair in Connections)
                {
                    if (pointPair.IndexA == previousIndex)
                        firstPair = pointPair;

                    if (pointPair.IndexA == newIndex)
                        secondPair = pointPair;
                }

                if (firstPair != null)
                    firstPair.IndexA = newIndex;

                if (secondPair != null)
                    secondPair.IndexA = previousIndex;
            }
            else if (group == EPointGroup.B)
            {
                foreach (PointPair pointPair in Connections)
                {
                    if (pointPair.IndexB == previousIndex)
                        firstPair = pointPair;

                    if (pointPair.IndexB == newIndex)
                        secondPair = pointPair;
                }

                if (firstPair != null)
                    firstPair.IndexB = newIndex;

                if (secondPair != null)
                    secondPair.IndexB = previousIndex;
            }

            CalcCurrentSum();
        }
    }
}