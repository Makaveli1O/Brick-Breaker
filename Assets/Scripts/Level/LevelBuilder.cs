using System;
using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Unity.Mathematics;

namespace Assets.Scripts.Level
{
    public class LevelBuilder
    {
        private readonly List<BlockData> _entries = new();

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


        public LevelData Build()
        {
            return new LevelData { Blocks = _entries };
        }
    }

}