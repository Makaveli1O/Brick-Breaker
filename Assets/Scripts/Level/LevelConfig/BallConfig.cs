using UnityEngine;

namespace Assets.Scripts.Level.Config
{
    public record BallConfig
    {
        public Vector2? LaunchDirection { get; set; } = null;
        public Vector2? BallInitialPosition { get; set; } = null;
        public float? BallInitialPush { get; set; } = null;
    }
}