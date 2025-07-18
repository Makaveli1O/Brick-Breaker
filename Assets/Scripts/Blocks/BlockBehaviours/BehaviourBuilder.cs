using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.Blocks
{
    public class BehaviourBuilder
    {
        private readonly List<BehaviourConfig> _configs = new();

        public BehaviourBuilder Add<TBehaviour, TConfig>(TConfig config)
        {
            _configs.Add(new BehaviourConfig(typeof(TBehaviour), config!));
            return this;
        }

        public BehaviourBuilder AddNonConfigurable<TBehaviour>()
        {
            if (IsBehaviourConfigurable<TBehaviour>())
                throw new Exception("Added behaviour requires config associated with it.");

            _configs.Add(new BehaviourConfig(typeof(TBehaviour), NoConfig.Instance));
            return this;
        }

        private bool IsBehaviourConfigurable<TBehaviour>()
        {
                var behaviourType = typeof(TBehaviour);
                var configurableInterface = typeof(IConfigurableBehaviour<>);

                return behaviourType
                    .GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == configurableInterface);
        }

        public List<BehaviourConfig> Build() => _configs;
    }
}