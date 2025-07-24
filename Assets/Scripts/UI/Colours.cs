using UnityEngine;

/*
Name	Hex	new Color(r, g, b)
Powder Pink	#FFD6E8	new Color(1.0f, 0.839f, 0.91f)
*/
namespace Assets.Scripts.UI
{
    public class Colours
    {
        public static readonly Color BasicBlockColor = new(0.89f, 0.788f, 1.0f); // #E3C9FF
        public static readonly Color SlowBlockColor = new(0.643f, 0.843f, 1.0f); // #A4D7FF
        public static readonly Color ButtonTextColor = new(1f, 1f, 1f); // #ffffff
        public static readonly Color ButtonTextColorHover = new(1.0f, 0.714f, 0.631f); // #FFB6A1
        public static readonly Color MoveBlockColor = new(1.0f, 0.796f, 0.643f); // #FFCBA4
        public static readonly Color ExplodeBlockColor = new(0.655f, 1.0f, 0.922f); // #A7FFEB
        public static readonly Color ReflectBlockColor = new(1.0f, 0.976f, 0.69f); // #FFF9B0
    } 
}
