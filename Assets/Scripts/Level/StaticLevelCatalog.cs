using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog : ILevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition(1, "Tutorial: Launch Basics", "Learn to move your paddle and launch the ball to destroy blocks."),
            new LevelDefinition(2, "Tutorial: Paddle Rotation", "A vertical wall blocks direct shots. Use A/D to rotate the paddle and aim your launch."),
            new LevelDefinition(3, "Tutorial: Speed Boost", "Your paddle is too slow! Use SPACEBAR to boost and catch the ball launched to the side."),
            new LevelDefinition(4, "Heart Loss Demo", "Shows what happens when the ball is missed. Lose a heart."),

        };
        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}