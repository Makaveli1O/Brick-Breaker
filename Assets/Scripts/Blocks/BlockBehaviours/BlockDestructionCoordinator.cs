using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class DestructionCoordinator : MonoBehaviour
    {
        private bool _isDestroying = false;
        private IShrapnelPoolFacade _pool;
        private readonly List<IEnumerator> _destructionCoroutines = new();

        public void RegisterCoroutine(IEnumerator coroutine)
        {
            _destructionCoroutines.Add(coroutine);
        }

        public void TriggerDestruction()
        {
            if (_isDestroying) return;
            _isDestroying = true;
            StartCoroutine(RunDestruction());
        }

        public void ReturnShrapnelToObjectPoolAfter(float seconds, GameObject go)
        {
            _pool = SimpleServiceLocator.Resolve<IShrapnelPoolFacade>();
            StartCoroutine(ReturnRoutine(seconds, go));
        }

        private IEnumerator RunDestruction()
        {
            while (true)
            {
                var routines = new List<IEnumerator>(_destructionCoroutines);
                _destructionCoroutines.Clear();

                foreach (var routine in routines)
                    yield return StartCoroutine(routine);

                if (_destructionCoroutines.Count == 0)
                    break;
            }

            Destroy(gameObject);
        }

        private IEnumerator ReturnRoutine(float seconds, GameObject go)
        {
            yield return new WaitForSeconds(seconds);
            _pool?.Return(go);
        }
    }
}
