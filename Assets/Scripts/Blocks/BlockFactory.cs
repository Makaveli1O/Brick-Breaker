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
            try
            {
                BlockGrid.InitializeFrom(_blockPrefab);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Block prefab is not assigned");
            }
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

        public Sprite GetBlockSprite() => _blockPrefab.GetComponent<SpriteRenderer>().sprite;
    }

}