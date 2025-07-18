using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Powerups
{
    public class PowerupSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _powerupPrefabs;

        public void Spawn(int index, GameObject target, object config = null)
        {
            if (index < 0 || index >= _powerupPrefabs.Count) return;

            var prefab = _powerupPrefabs[index];
            var instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);

            if (instance.TryGetComponent<PowerupBase>(out var powerup))
            {
                powerup.Configure(config);
                powerup.Initialize(target);
            }
        }
    }
}
