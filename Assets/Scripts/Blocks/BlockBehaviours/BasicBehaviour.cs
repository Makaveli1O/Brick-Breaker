using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BasicBehaviour : MonoBehaviour, ICollisionBehaviour, IDestructableBehaviour
    {
        private IBlockCounter _blockCounter;
        void Awake()
        {
            _blockCounter = SimpleServiceLocator.Resolve<IBlockCounter>();
        }

        public void RegisterAndTrigger(Block context)
        {
            return;
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

        public void OnCollisionExecute(Block context, Collision2D collision)
        {
            DestroyBlock(context);
        }
    }
}