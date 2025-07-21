using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class PowerupExpirationCoordinator : MonoBehaviour, IPowerupExpirationCoordinator
    {
        private class TrackedPowerup
        {
            public PowerupBase Powerup;
            public float RemainingTime;
            public float TotalDuration;
        }

        private readonly List<TrackedPowerup> _tracked = new();

        public IReadOnlyList<(PowerupBase powerup, float remainingTime)> GetActivePowerups()
        {
            return _tracked
                .Select(t => (t.Powerup, t.RemainingTime))
                .ToList();
        }

        public void Track(PowerupBase powerup, Action<PowerupBase> onExpired)
        {
            var tracked = new TrackedPowerup
            {
                Powerup = powerup,
                TotalDuration = powerup.Duration,
                RemainingTime = powerup.Duration
            };
            _tracked.Add(tracked);
            StartCoroutine(WaitAndExpire(tracked, onExpired));
        }

        private IEnumerator WaitAndExpire(TrackedPowerup tracked, Action<PowerupBase> callback)
        {
            while (tracked.RemainingTime > 0f)
            {
                tracked.RemainingTime -= Time.deltaTime;
                yield return null;
            }

            _tracked.Remove(tracked);
            callback?.Invoke(tracked.Powerup);
        }
    }
}
