using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog : ILevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition(1, "Pilot"),
            new LevelDefinition(2, "Moving Challenge"),
            new LevelDefinition(3, "Explosion performance test"),
            new LevelDefinition(4, "Reflect behaviour test"),
            new LevelDefinition(5, "Slower madness")
        };

        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}