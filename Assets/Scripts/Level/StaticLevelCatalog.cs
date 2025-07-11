using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog : ILevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition(0, "Starter Level"),
            new LevelDefinition(1, "Exploding Madness"),
            new LevelDefinition(2, "Moving Challenge"),
            // Add more here
        };

        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}