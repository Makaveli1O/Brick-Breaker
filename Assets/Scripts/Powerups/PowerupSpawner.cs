using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.SharedKernel;
using System.Linq;
using System;

namespace Assets.Scripts.Powerups
{
    public class PowerupSpawner : MonoBehaviour
    {
        public enum PowerupTypes
        {
            SpeedBoost
        }

        [SerializeField] private GameObject _speedBoostPrefab;
        [SerializeField] private GameObject _container;

        private readonly Dictionary<PowerupTypes, GameObject> _prefabMap = new();
        private IPowerupExpirationCoordinator _coordinator;

        private void Awake()
        {
            _prefabMap[PowerupTypes.SpeedBoost] = _speedBoostPrefab;
        }

        private void Start()
        {
            _coordinator = SimpleServiceLocator.Resolve<IPowerupExpirationCoordinator>();
        }

        public void Spawn(PowerupTypes type, GameObject target, object config = null)
        {
            if (!_prefabMap.TryGetValue(type, out var prefab))
                throw new ArgumentException($"Prefab not defined for powerup type: {type}");

            if (_coordinator.GetActivePowerups().Any(p => p.powerup.Type == type))
            {
                Debug.Log($"Powerup already active: {type}");
                return;
            }

            var instance = Instantiate(prefab, transform.position, Quaternion.identity, _container.transform);

            if (!instance.TryGetComponent<PowerupBase>(out var powerup))
                throw new InvalidOperationException($"Prefab does not contain a PowerupBase: {prefab.name}");

            powerup.Configure(config);
            powerup.Initialize(target);
            _coordinator.Track(powerup, powerup.OnExpiration);
        }
    }
}
