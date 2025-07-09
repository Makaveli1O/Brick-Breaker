using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public record MoveConfig
    {
        public float Speed{ get; }
        public Vector3 EndPoint{ get; }
        public Vector3 StartPoint{ get; }
        public float PhaseOffset{ get; }

        public MoveConfig(float speed, Vector3 startPoint, Vector3 endPoint, float phaseOffset = 0f)
        {
            Speed = speed;
            StartPoint = startPoint;
            EndPoint = endPoint;
            PhaseOffset = phaseOffset;
        }
    }
}