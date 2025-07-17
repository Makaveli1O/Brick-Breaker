using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class ShrapnelPool : MonoBehaviour, IShrapnelPool
    {
        [SerializeField] private GameObject _shrapnelPrefab;
        [SerializeField] private int _initialSize = 50;

        private Queue<GameObject> _pool = new();

        void Awake()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                var go = Instantiate(_shrapnelPrefab);
                go.SetActive(false);
                _pool.Enqueue(go);
            }
        }

        public GameObject Get(Vector3 position)
        {
            GameObject go = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(_shrapnelPrefab);
            go.transform.position = position;
            go.SetActive(true);
            return go;
        }

        public void Return(GameObject shrapnel)
        {
            shrapnel.SetActive(false);
            _pool.Enqueue(shrapnel);
        }
    }

}