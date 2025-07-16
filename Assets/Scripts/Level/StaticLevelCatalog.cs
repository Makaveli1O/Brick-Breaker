using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog : ILevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition(1, "Pilot", "A balanced starter level showcasing basic moving and exploding blocks."),
            new LevelDefinition(2, "Instant finish", "Finishes instantly! Test purposes."),
            new LevelDefinition(3, "Explosion Performance Test", "Stress test: large grid of exploding blocks. Boom everywhere."),
            new LevelDefinition(4, "Reflect Behaviour Test", "A vertical wall of reflectors to redirect the ball. Watch your angles."),
            new LevelDefinition(5, "Slower Madness", "Dense cluster of slow blocks. Gameplay slows dramatically."),
            new LevelDefinition(6, "Slow & Reflect Cross Pattern", "Cross-shaped challenge using slow and reflect behaviours."),
            new LevelDefinition(7, "Slow + Explode Combo Grid", "Grid of hybrid blocks that slow and then explode. Chaos follows."),
        };
        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}