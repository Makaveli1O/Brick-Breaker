using UnityEngine;

namespace Assets.Scripts.Level
{
    public record LevelConfig
    {
        public Vector2? LaunchDirection { get; set; } = null;

        public LevelConfig(Vector2? launchDirection = null)
        {
            LaunchDirection = launchDirection;
        }
    }
}