using System.Collections;
using UnityEngine;
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.Blocks
{
    public class ShrapnelPoolFacade : ShrapnelPoolFacadeBase
    {
        [SerializeField] private GameObject _shrapnelPrefab;
        [SerializeField] private int _maxShrapnelCount = 50;
        private IObjectPool<GameObject> _pool;
        private Transform _container;
        private float _delaySeconds = 1f;

        void Awake()
        {
            _container = new GameObject("ShrapnelContainer").transform;

            _pool = new ObjectPool<GameObject>(() =>
            {
                var go = Instantiate(_shrapnelPrefab, _container);
                go.SetActive(false);
                return go;
            }, _maxShrapnelCount);
        }

        public override GameObject Get(Vector3 position)
        {
            GameObject go = _pool.Get();
            go.transform.position = position;
            go.SetActive(true);
            return go;
        }

        public override void Return(GameObject shrapnel)
        {
            shrapnel.SetActive(false);
            _pool.Return(shrapnel);
        }

        public override void ScheduleReturn(GameObject shrapnel)
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
