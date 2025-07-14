using NUnit.Framework.Internal;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class ReflectBehaviour : MonoBehaviour, ICollisionBehaviour, IConfigurableBehaviour<ReflectBehaviour>
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
            Vector2 normal = collision.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);

            rb.linearVelocity = reflectedVelocity;
        }

        public void Configure(ReflectBehaviour config)
        {
            ReflectDirection = config.ReflectDirection;
        }
    }
}