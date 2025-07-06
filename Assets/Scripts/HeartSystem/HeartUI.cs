using System.Collections.Generic;
using System.Dynamic;
using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.HeartSystem
{
    public class HeartUI : MonoBehaviour
    {
        [SerializeField] private GameObject _heartPrefab;
        private IHeartController _heartController;
        private const float _initialPositionX = -6.0f;
        private readonly List<HeartIcon> _icons = new();

        private class HeartIcon
        {
            public GameObject GameObject;
            public int Index;
        }

        void Start()
        {
            _heartController = SimpleServiceLocator.Resolve<IHeartController>();

            if (_heartController == null) throw new System.Exception("Heart controller not assigned!");
            InstantiateHearts();
        }

        private void InstantiateHearts()
        {
            _heartController.OnHeartRemoved += HandleHeartRemoved;
            _heartController.OnHeartAdded += HandleHeartAdded;

            for (int i = 0; i < _heartController.GetMaxHearts; i++)
            {
                GameObject go = Instantiate(_heartPrefab, transform.position, Quaternion.identity);

                HeartIcon icon = new()
                {
                    GameObject = go,
                    Index = i
                };
                icon.GameObject.transform.position = new Vector3(
                    _initialPositionX - i,
                    transform.position.y,
                    transform.position.z
                );
                
                _icons.Add(new HeartIcon
                    {
                        GameObject = go,
                        Index = i
                    }
                );
            }
        }

        private void HandleHeartRemoved(int newCount)
        {
            if (newCount >= 0 && newCount < _icons.Count)
                _icons[newCount].GameObject.SetActive(false);
        }

        private void HandleHeartAdded(int newCount)
        {
            if (newCount >= 0 && newCount < _icons.Count)
                _icons[newCount].GameObject.SetActive(true);
        }
    }
}