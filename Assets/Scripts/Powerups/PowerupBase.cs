using UnityEngine;
using System.Collections;
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.Powerups
{
    public abstract class PowerupBase : MonoBehaviour
    {
        protected GameObject _target;
        [SerializeField] protected AudioClip _sfx;
        protected ISoundPlayer _soundPlayer;

        public void Initialize(GameObject target)
        {
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _target = target;
            Apply();
            StartCoroutine(ExpireAfterDelay());
        }
        public virtual void Configure(object config) { }
        protected abstract void Apply();
        protected abstract void Revert();
        protected abstract float Duration { get; set; }
        protected virtual void SetDefaultConfiguration() { return; }

        private IEnumerator ExpireAfterDelay()
        {
            yield return new WaitForSeconds(Duration);
            Revert();
            Destroy(gameObject);
        }
    }
}
