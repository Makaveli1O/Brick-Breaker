using static Assets.Scripts.Powerups.PowerupSpawner;

namespace Assets.Scripts.Powerups
{
    public class SpeedBoost : PowerupBase
    {
        public override PowerupTypes Type => PowerupTypes.SpeedBoost;
        private float Multiplier;
        protected override float duration { get => _duration; set => _duration = value; }
        private float Acceleration;
        private float _originalSpeed;
        private float _originalAcceleration;
        private float _duration;
        private const float _defaultMultiplier = 2f;
        private const float _defaultDuration = 5f;
        private const float _defaultAcceleration = 20f;

        public override void Configure(object config)
        {
            if (config is SpeedBoostConfig boostConfig)
            {
                Multiplier = boostConfig.Multiplier;
                duration = boostConfig.Duration;
            }
            else
            {
                SetDefaultConfiguration();
            }
        }

        protected override void SetDefaultConfiguration()
        {
            Multiplier = _defaultMultiplier;
            duration = _defaultDuration;
            Acceleration = _defaultAcceleration;
        }

        protected override void Apply()
        {
            if (!_target.TryGetComponent<IPaddleBehaviour>(out var controller)) return;
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

