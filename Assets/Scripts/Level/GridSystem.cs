using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class GridSystem
    {
        private readonly float _cellWidth;
        private readonly float _cellHeight;

        public GridSystem(float cellWidth, float cellHeight)
        {
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
        }

        public float2 ToWorldPosition(int x, int y)
        {
            return new float2(x * _cellWidth, y * _cellHeight);
        }

        public float2 SnapToGrid(float2 position)
        {
            int x = Mathf.RoundToInt(position.x / _cellWidth);
            int y = Mathf.RoundToInt(position.y / _cellHeight);
            return ToWorldPosition(x, y);
        }
    }
}
