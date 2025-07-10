using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition("Level_001", "Starter Level"),
            new LevelDefinition("Level_002", "Exploding Madness"),
            new LevelDefinition("Level_003", "Moving Challenge"),
            // Add more here
        };

        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}