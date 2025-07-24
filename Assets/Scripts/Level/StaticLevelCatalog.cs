using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog : ILevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition(1, "Tutorial 1: Launch Basics", "Learn to move your paddle and launch the ball to destroy blocks."),
            new LevelDefinition(2, "Tutorial 2: Paddle Rotation", "A vertical wall blocks direct shots. Use Q/E to rotate the paddle and aim your launch."),

        };
        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}