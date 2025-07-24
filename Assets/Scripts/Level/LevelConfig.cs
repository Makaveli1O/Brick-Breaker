using UnityEngine;

namespace Assets.Scripts.Level
{
    public record LevelConfig
    {
        public Vector2? LaunchDirection { get; set; } = null;
        public Vector2? BallInitialPosition { get; set; } = null;
        public float? BallInitialPush { get; set; } = null;

        public LevelConfig(
            Vector2? launchDirection = null,
            Vector2? ballInitialPosition = null,
            float? ballInitialPush = null
        )
        {
            LaunchDirection = launchDirection;
            BallInitialPosition = ballInitialPosition;
            BallInitialPush = ballInitialPush;
        }
    }
}