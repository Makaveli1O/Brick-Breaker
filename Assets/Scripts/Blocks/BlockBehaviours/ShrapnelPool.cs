using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class ShrapnelPool : MonoBehaviour, IShrapnelPool
    {
        [SerializeField] private GameObject _shrapnelPrefab;
        [SerializeField] private int _maxShrapnelCount = 50;
        private Queue<GameObject> _pool = new();
        private GameObject _shrapnelContainer;
        private const float _delaySeconds = 2f;

        void Awake()
        {
            _shrapnelContainer = new GameObject("ShrapnelContainer");

            for (int i = 0; i < _maxShrapnelCount; i++)
            {
                var go = Instantiate(_shrapnelPrefab, _shrapnelContainer.transform);
                go.SetActive(false);
                _pool.Enqueue(go);
            }
        }

        public GameObject Get(Vector3 position)
        {
            if (_pool.Count == 0) return null;

            GameObject go = _pool.Dequeue();
            go.transform.position = position;
            go.SetActive(true);
            return go;
        }

        public void Return(GameObject shrapnel)
        {
            shrapnel.SetActive(false);
            _pool.Enqueue(shrapnel);
        }

        public void ScheduleReturn(GameObject shrapnel)
        {
            StartCoroutine(ReturnAfterDelay(shrapnel));
        }

        private IEnumerator ReturnAfterDelay(GameObject go)
        {
            yield return new WaitForSeconds(_delaySeconds);
            Return(go);
        }
    }
}