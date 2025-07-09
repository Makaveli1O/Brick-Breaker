using System.Collections.Generic;
using Unity.Mathematics;

namespace Assets.Scripts.Blocks
{
    /// <summary>
    /// Value object - represents the data for a block, including its behaviour, shape, colour, type, and position.
    /// </summary>
    public record BlockData
    {
        public BlockShape Shape { get; }
        public float2 Position { get; set; }
        public List<BehaviourConfig> Behaviours { get; set; }

        public BlockData(BlockShape shape, float2 position, List<BehaviourConfig> behaviours)
        {
            Shape = shape;
            Position = position;
            Behaviours = behaviours;
        }
    }
}
