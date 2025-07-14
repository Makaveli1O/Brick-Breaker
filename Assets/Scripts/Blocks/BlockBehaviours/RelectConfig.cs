using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public record ReflectConfig
    {
        public Vector2 ReflectDirection { get; }

        public ReflectConfig(Vector2 reflectDirection)
        {
            ReflectDirection = reflectDirection;
        }
    }
}