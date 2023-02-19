using System;
using System.Collections.Generic;
using SettingsModule;
using WireGameModule.Infrastructure;

namespace WireGameModule.Model
{
    public sealed class WireGameLevelHolder
    {
        private readonly GameSettings _gameSettings;
        private readonly Dictionary<int, WireGameLevelData> _levels = new();

        public WireGameLevelHolder(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public WireGameLevelData GetLevel(int levelIndex)
        {
            if (_levels.TryGetValue(levelIndex, out var levelData))
                return levelData;

            List<WireGameLevel> levels = _gameSettings.WireLevels;
            if (levelIndex < 0 || levelIndex >= levels.Count)
                throw new Exception($"Unavailable level index '{levelIndex}' for '{levels.Count}' levels");

            WireGameLevel level = levels[levelIndex];

            var wireGameLevelData = new WireGameLevelData(level);
            _levels[levelIndex] = wireGameLevelData;
            return wireGameLevelData;
        }
    }
}