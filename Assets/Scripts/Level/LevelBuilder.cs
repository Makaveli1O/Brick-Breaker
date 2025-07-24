using System;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Unity.Mathematics;
using Assets.Scripts.Level.Config;

namespace Assets.Scripts.Level
{
    public class LevelBuilder
    {
        private readonly List<BlockData> _entries = new();
        public LevelConfig LevelConfig { get; set; }

        public LevelBuilder WithBlock(
            float2 position,
            List<BehaviourConfig> behaviourConfigs
        )
        {
            if (behaviourConfigs == null)
                throw new ArgumentNullException(nameof(behaviourConfigs));

            _entries.Add(
                new BlockData(
                    null,
                    position,
                    behaviourConfigs
                )
            );
            return this;
        }

        public LevelBuilder WithBlock(
            int gridX,
            int gridY,
            List<BehaviourConfig> behaviourConfigs,
            GridSystem grid
        )
        {
            float2 snappedPosition = grid.ToWorldPosition(gridX, gridY);
            return WithBlock(snappedPosition, behaviourConfigs);
        }

        public LevelBuilder WithConfig(LevelConfig levelConfig)
        {
            LevelConfig = levelConfig;
            return this;
        }

        public LevelData Build()
        {
            return new LevelData { Blocks = _entries, LevelConfig = LevelConfig };
        }
    }

}