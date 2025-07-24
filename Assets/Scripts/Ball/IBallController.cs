using UnityEngine;

namespace Assets.Scripts.Ball{
    public interface IBallController
    {
        public void LaunchBall();
        public void LaunchBall(Vector2 direction);

        public void Deactivate();
        public void Activate();
    }
}