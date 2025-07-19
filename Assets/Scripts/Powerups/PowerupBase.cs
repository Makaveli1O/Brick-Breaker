using UnityEngine;
using System.Collections;
using Assets.Scripts.SharedKernel;
using UnityEngine.UI;
using static Assets.Scripts.Powerups.PowerupSpawner;

namespace Assets.Scripts.Powerups
{
    public abstract class PowerupBase : MonoBehaviour
    {
        protected GameObject _target;
        public abstract PowerupTypes Type { get; }
        [SerializeField] private Sprite _icon;
        [SerializeField] protected AudioClip _sfx;
        protected ISoundPlayer _soundPlayer;

        public void Initialize(GameObject target)
        {
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _target = target;
            SetSpriteIcon();
            Apply();
        }
        public virtual void Configure(object config) { }
        protected abstract void Apply();
        protected abstract void Revert();
        public float Duration => duration;
        protected abstract float duration { get; set; }
        protected virtual void SetDefaultConfiguration() { return; }
        private void SetSpriteIcon() => GetComponent<Image>().sprite = _icon;

        public void OnExpiration(PowerupBase powerup)
        {
            Revert();
            Destroy(gameObject);
        }
    }
}
