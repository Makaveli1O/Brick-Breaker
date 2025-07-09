using System;
using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BlockFactory : MonoBehaviour, IBlockFactory
    {
        [SerializeField] private GameObject _blockPrefab;
        void Awake()
        {
            BlockGrid.InitializeFrom(_blockPrefab);
        }

        public Block SpawnBlock(BlockData data, Transform parent)
        {
            if (_blockPrefab == null) throw new Exception("Block prefab not assigned.");

            GameObject go = Instantiate(
                _blockPrefab,
                Utils2D.PositionConvertor2D.ToVector2(data.Position),
                Quaternion.identity,
                parent
            );

            Block block = new BlockBuilder(go)
                .AddBehaviours(data.Behaviours)
                .WithData(data)
                .WithColour(BlockColourResolver.Resolve(data.Behaviours))
                .Build();

            return block;
        }
    }

}