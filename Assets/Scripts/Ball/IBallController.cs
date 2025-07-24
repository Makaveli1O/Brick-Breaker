using UnityEngine;

namespace Assets.Scripts.Ball{
    public interface IBallController
    {
        public void LaunchBall();
        public void SetLaunchDirection(Vector2? launchDirection);
        public void SetInitialPosition(Vector2? initialPosition);
        public void SetInitialPush(float? initialPush);
        public void Deactivate();
        public void Activate();
    }
}