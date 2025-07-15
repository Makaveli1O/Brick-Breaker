namespace Assets.Scripts.Blocks
{
    public interface IDestructableBehaviour
    {
        public void AdjustBlockCounter();
        public void DestroyBlock(Block context);
        public void RegisterAndTrigger(Block context); // Register and trigger destruction
    }
}