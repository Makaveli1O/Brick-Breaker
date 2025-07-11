namespace Assets.Scripts.Blocks
{
    // This interface defines a behaviour that can be updated every tick.
    public interface IUpdateBehaviour : IBlockBehaviour
    { 
        void OnUpdateExecute(Block context);
    }
}