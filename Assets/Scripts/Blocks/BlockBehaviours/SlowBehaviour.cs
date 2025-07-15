using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class SlowBehaviour : MonoBehaviour, IDestructableBehaviour, IConfigurableBehaviour<SlowConfig>, ICollisionBehaviour
    {
        public float Slowness { get; set; } = 0.5f;
        private bool _isSlowed = false;
        private const float _delay = 2f;
        private const float _normalSpeed = 1f;
        public void Configure(SlowConfig config)
        {
            Slowness = config.SlownessPercentage;
        }

        public void OnCollisionExecute(Block context, Collision2D collision)
        {
            if (_isSlowed) return;

            Time.timeScale = Slowness;
            _isSlowed = true;

            context.StartCoroutine(RestoreTimeAfterDelay(_delay));
        }

        private System.Collections.IEnumerator RestoreTimeAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            Time.timeScale = _normalSpeed;
            _isSlowed = false;
        }

        public void DestroyBlock(Block context)
        {
            AdjustBlockCounter();
            Destroy(context.gameObject);
        }

        public void AdjustBlockCounter()
        {
            SimpleServiceLocator.Resolve<IBlockCounter>().OnBlockDestroyed();
        }
    }
}