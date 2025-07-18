using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public abstract class PowerupBase : MonoBehaviour
    {
        protected GameObject _target;
        protected float _duration;
        protected abstract void Apply();
        protected abstract void Revert();

        protected IEnumerator ExpireAfterDelay()
        {
            yield return new WaitForSeconds(_duration);
            Revert();
            Destroy(gameObject);
        }
    }
}
