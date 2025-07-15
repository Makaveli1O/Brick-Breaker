using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public record SlowConfig
    {
        public float Duration { get; }
        public float SlownessPercentage { get; }

        public SlowConfig(float duration, float slownessPercentage)
        {
            Duration = duration;
            SlownessPercentage = Mathf.Clamp01(slownessPercentage);
        }
    }
}