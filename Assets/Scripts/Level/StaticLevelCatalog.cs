using System.Collections.Generic;

namespace Assets.Scripts.Level
{
    public class StaticLevelCatalog : ILevelCatalog
    {
        private readonly List<LevelDefinition> _levels = new()
        {
            new LevelDefinition(1, "Tutorial1: Basic movement.", "Basic level with a row of moving blocks and a wall of exploding blocks."),
            new LevelDefinition(2, "Tutorial2: Move block", "These blocks move! Time your shots."),
            new LevelDefinition(3, "Tutorial3: Explosion block", "Grid of only exploding blocks."),
            new LevelDefinition(4, "Tutorial4: Reflect block", "Single vertical wall made of reflect blocks. Demonstrates bounce logic."),
            new LevelDefinition(5, "Tutorial5: Combination", "Compact cluster of slow blocks that heavily reduce ball speed."),
            new LevelDefinition(6, "Tutorial6: Slo reflect block", "Cross shape made from reflect (vertical) and slow (horizontal) blocks."),
            new LevelDefinition(7, "Tutorial7: Slow + explosion", "5x5 grid of blocks combining slow and explode behaviour."),
        };
        public IReadOnlyList<LevelDefinition> GetAvailableLevels() => _levels;
    }
}