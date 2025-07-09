using System;
using System.Collections.Generic;
using Assets.Scripts.SharedKernel;
using UnityEngine;

/*
Name	Hex	new Color(r, g, b)
Peach	#FFCBA4	new Color(1.0f, 0.796f, 0.643f)
Light Mint	#A7FFEB	new Color(0.655f, 1.0f, 0.922f)
Lavender	#E3C9FF	new Color(0.89f, 0.788f, 1.0f)
Sky Blue	#A4D7FF	new Color(0.643f, 0.843f, 1.0f)
Pale Yellow	#FFF9B0	new Color(1.0f, 0.976f, 0.69f)
Powder Pink	#FFD6E8	new Color(1.0f, 0.839f, 0.91f)
Light Coral	#FFB6A1	new Color(1.0f, 0.714f, 0.631f)
Bright Teal	#9FFFFF	new Color(0.624f, 1.0f, 1.0f)
*/
namespace Assets.Scripts.Blocks
{
    public class BlockColourResolver
    {
        private static readonly Dictionary<Type, Color> _behaviourColours = new()
        {
            [typeof(MoveBehaviour)] = new Color(1.0f, 0.796f, 0.643f),
            [typeof(ExplodeBehaviour)] = new Color(0.655f, 1.0f, 0.922f),
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