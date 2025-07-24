using UnityEngine;

namespace Assets.Scripts.Ball{
    public interface IBallController
    {
        public void LaunchBall();
        public void Deactivate();
        public void Activate();
    }
}