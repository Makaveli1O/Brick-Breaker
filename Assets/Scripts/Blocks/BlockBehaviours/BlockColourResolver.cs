using System;
using System.Collections.Generic;
using Assets.Scripts.SharedKernel;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BlockColourResolver
    {
        private static readonly Dictionary<Type, Color> _behaviourColours = new()
        {
            [typeof(MoveBehaviour)] = Colours.MoveBlockColor,
            [typeof(ExplodeBehaviour)] = Colours.ExplodeBlockColor,
            [typeof(ReflectBehaviour)] = Colours.ReflectBlockColor,
            [typeof(SlowBehaviour)] = Colours.SlowBlockColor
        };
        public static Color Resolve(List<BehaviourConfig> behaviours)
        {
            var colours = new List<Color>();

            foreach (var behaviour in behaviours)
            {
                if (_behaviourColours.TryGetValue(behaviour.BehaviourType, out var blockColour))
                    colours.Add(blockColour);
            }

            return Utils2D.BlendColours(colours);
        }
    }
}