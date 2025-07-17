using System.Collections;
using Assets.Scripts.SharedKernel;
using UnityEngine;
namespace Assets.Scripts.Blocks
{
    public class ExplodeBehaviour : MonoBehaviour, ICollisionBehaviour, IDestructableBehaviour
    {
        private AudioClip _explodeClip;
        private AudioClip _blip;
        private const int _shrapnelCount = 5;
        private const int _delaySeconds = 2;
        private const float _shrapnelOffset = 0.5f;
        
        private const float _shrapnelMass = 0.1f;
        private const float _blinkDelay = 0.1f;
        private float _spreadForce = 5f;
        private ISoundPlayer _soundPlayer;
        private IBlockCounter _blockCounter;
        private bool _isExploding = false;
        private IShrapnelPoolFacade _shrapnelPool;

        void Awake()
        {
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _explodeClip = Resources.Load<AudioClip>("Sound/Block/explosion");
            _blip = Resources.Load<AudioClip>("Sound/Block/blip");
            _blockCounter = SimpleServiceLocator.Resolve<IBlockCounter>();
            _shrapnelPool = SimpleServiceLocator.Resolve<IShrapnelPoolFacade>();
        }

        public void OnCollisionExecute(Block context, Collision2D collision)
        {
            RegisterAndTrigger(context);
        }

        public void Explode(Block context)
        {
            if (_isExploding) return;
            _isExploding = true;
            context.StartCoroutine(ExplosionSequence(context));
        }

        private IEnumerator ExplosionSequence(Block ctx)
        {
            // Blink Effect
            float elapsed = 0f;
            SpriteRenderer sr = ctx.GetComponent<SpriteRenderer>();
            Color originalColor = sr.color;

            while (elapsed < _delaySeconds)
            {
                sr.color = Color.white;
                yield return new WaitForSeconds(_blinkDelay);
                sr.color = originalColor;
                yield return new WaitForSeconds(_blinkDelay);
                elapsed += 2 * _blinkDelay;
                _soundPlayer.PlaySfx(_blip);
            }
            ExecuteExplosion(ctx);

            _isExploding = false;
        }

        private void ExecuteExplosion(Block ctx)
        {
            for (int i = 0; i < _shrapnelCount; i++)
            {
                Vector2 dir = Random.insideUnitCircle.normalized;
                Vector3 spawnPos = ctx.transform.position + (Vector3)(dir * _shrapnelOffset);

                GameObject shrapnel = _shrapnelPool.Get(spawnPos);

                if (shrapnel == null) return; //pool exhausted

                Rigidbody2D rb = shrapnel.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.mass = _shrapnelMass;
                    rb.AddForce(dir * _spreadForce, ForceMode2D.Impulse);
                }

                _shrapnelPool.ScheduleReturn(shrapnel);
            }
            _soundPlayer.PlaySfx(_explodeClip);
            DestroyBlock(ctx);
        }

        public void RegisterAndTrigger(Block context)
        {
            var coordinator = context.GetComponent<DestructionCoordinator>();
            coordinator.RegisterCoroutine(ExplosionSequence(context));
            coordinator.TriggerDestruction();
        }

        public void DestroyBlock(Block context)
        {
            AdjustBlockCounter();
            Destroy(context.gameObject);
        }

        public void AdjustBlockCounter()
        {
            _blockCounter.OnBlockDestroyed();
        }
    }
}