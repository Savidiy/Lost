using System;

namespace WireGameModule.Infrastructure
{
    [Serializable]
    public class PointPair
    {
        public int IndexA;
        public int IndexB;

        public PointPair(int indexA, int indexB)
        {
            IndexA = indexA;
            IndexB = indexB;
        }

        public bool EqualsPair(PointPair other)
        {
            return IndexA == other.IndexA && IndexB == other.IndexB;
        }
    }
}