namespace Assets.Scripts.Blocks
{
    public interface IDestructableBehaviour
    {
        public void AdjustBlockCounter();
        void DestroyBlock(Block context);
    }
}