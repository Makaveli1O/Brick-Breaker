using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.Blocks
{
    public class BlockBuilder
    {
        private readonly GameObject _go;
        private readonly Block _block;

        public BlockBuilder(GameObject go)
        {
            _go = go;
            _block = go.GetComponent<Block>();
        }

        public BlockBuilder WithData(BlockData data)
        {
            _block.SetData(data);
            return this;
        }

        public BlockBuilder AddBehaviours(List<BehaviourConfig> behaviourConfigs)
        {
            foreach (var config in behaviourConfigs)
            {
                var instance = (MonoBehaviour)_go.AddComponent(config.BehaviourType);

                if (config.Config is not NoConfig)
                    ApplyConfig(instance, config.Config);

                if (instance is IUpdateBehaviour update)
                    _block.AddUpdateBehaviour(update);
                if (instance is ICollisionBehaviour collision)
                    _block.AddCollisionBehaviour(collision);
            }

            return this;
        }

        public BlockBuilder WithColour(Color color)
        {
            _block.SetColour(color);
            return this;
        }

        public Block Build()
        {
            return _block;
        }

        private void ApplyConfig(MonoBehaviour instance, object config)
        {
            var configType = config.GetType();
            var iface = typeof(IConfigurableBehaviour<>).MakeGenericType(configType);

            if (!iface.IsAssignableFrom(instance.GetType()))
                throw new Exception($"{instance.GetType().Name} does not implement IConfigurable<{configType.Name}>");

            var method = iface.GetMethod("Configure");
            method?.Invoke(instance, new[] { config });
        }
    }
}
