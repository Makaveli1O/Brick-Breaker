using System;
using System.Collections.Generic;

namespace Assets.Scripts.Powerups
{
    public interface IPowerupExpirationCoordinator
    {
        void Track(PowerupBase powerup, Action<PowerupBase> onExpired);
        public IReadOnlyList<(PowerupBase powerup, float remainingTime)> GetActivePowerups();
    }
}
