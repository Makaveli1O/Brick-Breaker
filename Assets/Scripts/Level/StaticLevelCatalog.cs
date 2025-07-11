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
            new LevelDefinition(3, "Test3"),
            new LevelDefinition(4, "Test4"),
            new LevelDefinition(5, "Test5"),
            new LevelDefinition(6, "Test6"),
            new LevelDefinition(7, "Test7"),
            new LevelDefinition(8, "Test8"),
            new LevelDefinition(9, "Test9"),
            new LevelDefinition(10, "Test10"),
            new LevelDefinition(11, "Test11"),
            new LevelDefinition(12, "Test12"),
            new LevelDefinition(13, "Test13"),
            new LevelDefinition(14, "Test14"),
            new LevelDefinition(15, "Test15"),
            new LevelDefinition(16, "Test16"),
            new LevelDefinition(17, "Test17"),
            new LevelDefinition(18, "Test18"),
            new LevelDefinition(19, "Test19"),
            new LevelDefinition(20, "Test20"),
            new LevelDefinition(21, "Test21"),
        };

        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}