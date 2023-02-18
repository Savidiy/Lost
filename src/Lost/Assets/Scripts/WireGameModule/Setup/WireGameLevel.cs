using System;
using System.Collections.Generic;
using UnityEngine;

namespace WireGameModule.Setup
{
    [CreateAssetMenu(fileName = "WireGameLevel", menuName = "WireGameLevel", order = 0)]
    public class WireGameLevel : ScriptableObject
    {
        public Sprite BackSprite;

        public List<Vector3> PointsA = new();
        public List<Vector3> PointsB = new();
        public List<PointPair> StartConnections = new();

        public int[,] ConnectsValue = new int[0, 0];
    }

    [Serializable]
    public class PointPair
    {
        public int IndexA;
        public int IndexB;
    }
}