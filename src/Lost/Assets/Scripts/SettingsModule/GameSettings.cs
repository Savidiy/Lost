using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WireGameModule.Infrastructure;

namespace SettingsModule
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        [Title("Connect points")]
        public Color ConnectPointUnusedColor;
        public Color ConnectPointUsedColor;
        public Color ConnectPointSelectedColor;
        public Color ConnectPointAvailableColor;
        
        [Title("Wires")]
        public float WireY;
        public float WireZ;
        public List<WireGameLevel> WireLevels;
        public int CurrentLevel;
        public List<Color> WireColors;
    }
}