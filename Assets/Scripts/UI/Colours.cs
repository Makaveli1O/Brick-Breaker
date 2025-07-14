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
namespace Assets.Scripts.UI
{
    public class Colours
    {
        public static readonly Color ButtonTextColor = new(1f, 1f, 1f);
        public static readonly Color ButtonTextColorHover = new(1.0f, 0.714f, 0.631f);
        public static readonly Color MoveBlockColor = new(1.0f, 0.796f, 0.643f);
        public static readonly Color ExplodeBlockColor = new(0.655f, 1.0f, 0.922f);
        public static readonly Color ReflectBlockColor = new(0.624f, 1.0f, 1.0f);
    } 
}
