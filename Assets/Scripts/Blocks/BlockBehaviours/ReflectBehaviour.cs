using UnityEngine;
using Assets.Scripts.SharedKernel;

namespace Assets.Scripts.Blocks
{
    public class ReflectBehaviour : MonoBehaviour, ICollisionBehaviour, IConfigurableBehaviour<ReflectConfig>
    {
        public Vector2 ReflectDirection { get; set; } = Vector2.left;
        public void OnCollisionExecute(Block context, Collision2D collision)
        {
            Reflect(collision);
        }

        private void Reflect(Collision2D collision)
        {
            var rb = collision.rigidbody;
            if (rb == null) return;

            Vector2 incomingVelocity = collision.relativeVelocity;
            float speed = incomingVelocity.magnitude;

            Vector2 direction = ReflectDirection.normalized;
            Vector2 reflectedVelocity = direction * speed;

            rb.linearVelocity = reflectedVelocity;
        }

        public void Configure(ReflectConfig config)
        {
            ReflectDirection = config.ReflectDirection;
        }
    }
}