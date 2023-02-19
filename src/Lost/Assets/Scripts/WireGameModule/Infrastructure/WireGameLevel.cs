using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WireGameModule.Infrastructure
{
    [CreateAssetMenu(fileName = "WireGameLevel", menuName = "WireGameLevel", order = 0)]
    public class WireGameLevel : SerializedScriptableObject
    {
        public Sprite BackSprite;

        public List<Vector3> PointsA = new();
        public List<Vector3> PointsB = new();
        public List<PointPair> StartConnections = new();
        public int TargetSum;

        [TableMatrix(HorizontalTitle = "A points", VerticalTitle = "B points")]
        public int[,] ConnectsValue = new int[0, 0];
    }
}