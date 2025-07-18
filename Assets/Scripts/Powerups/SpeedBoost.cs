namespace Assets.Scripts.Powerups
{
    public class SpeedBoost : PowerupBase
    {
        private float Multiplier;
        protected override float Duration { get => _duration; set => _duration = value; }
        private float Acceleration;
        private float _originalSpeed;
        private float _originalAcceleration;
        private float _duration;
        private const float _defaultMultiplier = 10f;
        private const float _defaultDuration = 5f;
        private const float _defaultAcceleration = 30f;

        public override void Configure(object config)
        {
            if (config is SpeedBoostConfig boostConfig)
            {
                Multiplier = boostConfig.Multiplier;
                Duration = boostConfig.Duration;
            }
            else
            {
                SetDefaultConfiguration();
            }
        }

        protected override void SetDefaultConfiguration()
        {
            Multiplier = _defaultMultiplier;
            Duration = _defaultDuration;
            Acceleration = _defaultAcceleration;
        }

        protected override void Apply()
        {
            var controller = _target.GetComponent<IPaddleBehaviour>();
            if (controller == null) return;
            _soundPlayer.PlaySfx(_sfx);

            _originalSpeed = controller.Speed;
            _originalAcceleration = controller.Acceleration;
            controller.Speed *= Multiplier;
            controller.Acceleration = Acceleration;
        }

        protected override void Revert()
        {
            var controller = _target.GetComponent<IPaddleBehaviour>();
            if (controller == null) return;

            controller.Speed = _originalSpeed;
            controller.Acceleration = _originalAcceleration;
        }
    }

}

