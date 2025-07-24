using System.Collections.Generic;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.Level.Config
{
    // List of individual block details within level
    public class LevelData
    {
        public LevelConfig LevelConfig{ get; set; }
        public List<BlockData> Blocks { get; set; } = new();
    }
}