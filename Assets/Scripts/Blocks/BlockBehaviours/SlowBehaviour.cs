using Assets.Scripts.Ball;
using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class SlowBehaviour : MonoBehaviour, IDestructableBehaviour, IConfigurableBehaviour<SlowConfig>, ICollisionBehaviour
    {
        public float Slowness { get; set; } = 0.5f;
        private const float _delay = 2f;

        public void Configure(SlowConfig config)
        {
            Slowness = config.SlownessPercentage;
        }

        public void OnCollisionExecute(Block context, Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Ball")) return;
            if (!collision.gameObject.TryGetComponent(out BallController ballController)) return;
            if (!collision.gameObject.TryGetComponent(out Rigidbody2D ballRigidbody)) return;

            if (!ballController.IsSlowed)
            {
                ballController.IsSlowed = true;

                Vector2 originalVelocity = ballRigidbody.linearVelocity;
                ballRigidbody.linearVelocity = originalVelocity * Slowness;

                ballController.StartCoroutine(RestoreSpeedAfterDelay(ballRigidbody, ballController, originalVelocity, _delay));
            }

            DestroyBlock(context);
        }

        private System.Collections.IEnumerator RestoreSpeedAfterDelay(Rigidbody2D rb, BallController controller, Vector2 originalVelocity, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            // Calculate speed ratio between original and current velocity
            float originalSpeed = originalVelocity.magnitude;
            float currentSpeed = rb.linearVelocity.magnitude;

            if (currentSpeed > 0)
            {
                // Restore velocity direction but keep the current velocity's direction
                Vector2 direction = rb.linearVelocity.normalized;
                rb.linearVelocity = direction * originalSpeed;
            }
            else
            {
                // If ball stopped, restore original velocity
                rb.linearVelocity = originalVelocity;
            }

            controller.IsSlowed = false;
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