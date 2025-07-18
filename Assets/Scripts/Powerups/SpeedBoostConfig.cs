namespace Assets.Scripts.Powerups
{
    public class SpeedBoostConfig : IPowerupConfig
    {
        public float Multiplier { get ; set; }
        public float Duration { get; set; }

        public SpeedBoostConfig(float multiplier, float duration)
        {
            Multiplier = multiplier;
            Duration = duration;
        }
    }
}
