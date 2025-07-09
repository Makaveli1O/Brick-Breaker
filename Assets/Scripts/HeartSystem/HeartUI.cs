using System.Collections.Generic;
using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.HeartSystem
{
    public class HeartUI : MonoBehaviour
    {
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private AudioClip _removeHeartClip;
        private ISoundPlayer _soundPlayer;
        private IHeartController _heartController;
        private const float _initialPositionX = -100.0f;
        private const float _positionOffset = 100f;
        private readonly List<HeartIcon> _icons = new();

        private class HeartIcon
        {
            public GameObject GameObject;
            public int Index;
        }

        void Start()
        {
            _heartController = SimpleServiceLocator.Resolve<IHeartController>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();

            if (_heartController == null) throw new System.Exception("Heart controller not assigned!");
            InstantiateHearts();
        }

        private void InstantiateHearts()
        {
            _heartController.OnHeartRemoved += HandleHeartRemoved;
            _heartController.OnHeartAdded += HandleHeartAdded;

            for (int i = 0; i < _heartController.GetMaxHearts; i++)
            {
                GameObject go = Instantiate(_heartPrefab, transform);

                HeartIcon icon = new()
                {
                    GameObject = go,
                    Index = i
                };
                icon.GameObject.transform.localPosition = new Vector3(
                    _initialPositionX + (i * _positionOffset),
                    0f,
                    1f
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
            {
                _icons[newCount].GameObject.SetActive(false);
                _soundPlayer.PlaySfx(_removeHeartClip);
            }
        }

        private void HandleHeartAdded(int newCount)
        {
            if (newCount >= 0 && newCount < _icons.Count)
                _icons[newCount].GameObject.SetActive(true);
        }
    }
}