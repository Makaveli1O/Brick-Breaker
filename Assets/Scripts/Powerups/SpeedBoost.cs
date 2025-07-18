
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.Powerups
{
    public class SpeedBoost : PowerupBase, IConfigurableBehaviour<SpeedBoostConfig>
    {
        private float _originalSpeed;
        public float Multiplier;
        public float Duration;

        public void Configure(SpeedBoostConfig config)
        {
            Multiplier = config.Multiplier;
            Duration = config.Duration;
        }

        protected override void Apply()
        {
            var controller = _target.GetComponent<IPaddleBehaviour>();
            if (controller == null) return;

            _originalSpeed = controller.Speed;
            controller.Speed *= Multiplier;
        }

        protected override void Revert()
        {
            var controller = _target.GetComponent<IPaddleBehaviour>();
            if (controller == null) return;

            controller.Speed = _originalSpeed;
        }
    }
}

