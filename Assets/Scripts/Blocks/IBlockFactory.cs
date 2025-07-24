using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public interface IBlockFactory
    {

        /// <summary>
        /// Spawns a block based on the provided block data.
        /// </summary>
        /// <param name="blockData">The data used to create the block.</param>
        /// <param name="parent">The parent transform under which the block will be spawned.</param>
        /// <returns>The spawned block.</returns>
        Block SpawnBlock(BlockData blockData, Transform parent);
        Sprite GetBlockSprite();
    }
}