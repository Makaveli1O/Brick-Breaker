using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts.Blocks;
using Unity.Mathematics;
using System.Reflection;
using Assets.Scripts.SharedKernel;
using System.Collections.Generic;
using Assets.Scripts.Score;

public class BlockIntegrationTest
{
    private const string collisionBehavioursField = "_collisionBehaviours";
    private const string updateBehavioursField = "_updateBehaviours";
    private GameObject blockSpawnerObject;
    private BlockSpawner blockSpawner;
    [SerializeField] public GameObject blockPrefab;
    private Camera _camera;

    [SetUp]
    public void SetUp()
    {
        GameObject camObj = new GameObject("TestCamera");
        _camera = camObj.AddComponent<Camera>();
        _camera.tag = "MainCamera";
        // Clear and register services BEFORE creating BlockSpawner
        SimpleServiceLocator.Clear();

        ScoreTracker st = new ();

        SimpleServiceLocator.Register<IScoreTracker>(st);

        // Load prefab
        var blockPrefab = Resources.Load<GameObject>("Prefabs/Blocks/Block");
        Assert.IsNotNull(blockPrefab, "Global Setup: Block prefab not found.");

        var blockCounter = new BlockWinConditionCounter();
        SimpleServiceLocator.Register<IBlockCounter>(blockCounter);

        var factoryGO = new GameObject("BlockFactory");
        var factory = factoryGO.AddComponent<BlockFactory>();
        typeof(BlockFactory)
            .GetField("_blockPrefab", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(factory, blockPrefab);
        SimpleServiceLocator.Register<IBlockFactory>(factory);

        blockSpawnerObject = new GameObject("BlockSpawner");
        blockSpawner = blockSpawnerObject.AddComponent<BlockSpawner>();
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(blockSpawnerObject);
        BlockWinConditionCounter counter = (BlockWinConditionCounter)SimpleServiceLocator.Resolve<IBlockCounter>();
        counter.ResetCounter();

        if (_camera != null)
            GameObject.DestroyImmediate(_camera.gameObject);
    }

    [UnityTest]
    public IEnumerator SpawnEmptyBlock_ShouldPass()
    {
        SpawnEmptyBlock(new float2(0, 0));
        yield return null; // Wait for Start/Awake

        var spawnedBlocks = GameObject.FindGameObjectsWithTag("Block");
        Assert.IsTrue(spawnedBlocks.Length > 0, "No blocks were spawned on start.");
    }

    [UnityTest]
    public IEnumerator SpawnAndDestroyEmptyBlock_ShouldPass()
    {
        // Spawn a block
        var block = SpawnEmptyBlock(new float2(0, 0));
        yield return null; // Wait a frame for Start/Awake

        Assert.IsNotNull(block, "Block was not spawned successfully.");
        // Destroy the block
        blockSpawner.DestroyBlock(block);
        yield return null; // Wait a frame for destruction

        var remainingBlocks = GameObject.FindGameObjectsWithTag("Block");
        Assert.IsTrue(remainingBlocks.Length == 0, "Block was not destroyed successfully.");
    }

    [UnityTest]
    public IEnumerator BlockSpawner_RemovesBlocks_WhenDestroyed()
    {
        yield return null; // Wait for blocks to spawn

        var spawnedBlocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (var block in spawnedBlocks)
        {
            UnityEngine.Object.Destroy(block);
        }
        yield return null;

        var remainingBlocks = GameObject.FindGameObjectsWithTag("Block");
        Assert.IsTrue(remainingBlocks.Length == 0, "Blocks were not removed after destruction.");
    }

    [UnityTest]
    public IEnumerator SpawnMultipleBlocks_ShouldSpawnCorrectNumber()
    {
        int spawnCount = 5;
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEmptyBlock(new float2(i, 0));
        }
        yield return null;

        var spawnedBlocks = GameObject.FindGameObjectsWithTag("Block");
        Assert.AreEqual(spawnCount, spawnedBlocks.Length, $"Expected {spawnCount} blocks to be spawned.");
    }

    [UnityTest]
    public IEnumerator DestroyBlock_NullReference_ShouldThrow()
    {
        Assert.Throws<System.Exception>(() => blockSpawner.DestroyBlock(null));
        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnBlock_WithCustomPosition_SetsCorrectTransform()
    {
        var position = new float2(3, 7);
        var block = SpawnEmptyBlock(position);
        yield return null;

        Assert.IsNotNull(block, "Block was not spawned.");
        Assert.AreEqual(position.x, Mathf.RoundToInt(block.transform.position.x), "Block X position mismatch.");
        Assert.AreEqual(position.y, Mathf.RoundToInt(block.transform.position.y), "Block Y position mismatch.");
    }

    [UnityTest]
    public IEnumerator DestroyAllBlocks_RemovesAllBlocks()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnEmptyBlock(new float2(i, 0));
        }
        yield return null;

        var spawnedBlocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (var blockObj in spawnedBlocks)
        {
            Block block = blockObj.GetComponent<Block>();
            blockSpawner.DestroyBlock(block);
        }
        yield return null;

        var remainingBlocks = GameObject.FindGameObjectsWithTag("Block");
        Assert.AreEqual(0, remainingBlocks.Length, "Not all blocks were destroyed.");
    }

    [UnityTest]
    public IEnumerator SpawnBlock_WithConfiguredMoveBehaviour_ShouldWork()
    {
        var behavioursBlueBasic = new BehaviourBuilder()
            .Add<MoveBehaviour, MoveConfig>(
                new MoveConfig(
                    2.0f,
                    new Vector3(-3f, -3f, 0f),
                    new Vector3(1f, -3f, 0f)
                )
            )
            .Build();

        var block = blockSpawner.SpawnBlock(new BlockData(null, new float2(0, 0), behavioursBlueBasic));
        var move = block.GetComponent<MoveBehaviour>();
        yield return null;
        Assert.IsNotNull(move);
        Assert.AreEqual(2f, move.Speed);
        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveBehaviour_ConfiguresSpeedCorrectly()
    {
        var go = new GameObject();
        var move = go.AddComponent<MoveBehaviour>();

        move.Configure(new MoveConfig(3.5f, Vector3.zero, Vector3.zero));

        Assert.AreEqual(3.5f, move.Speed); // Assuming Speed is exposed for test

        yield return null;
    }

    [UnityTest]
    public IEnumerator BlockColourResolver_BlendsMoveAndExplodeColours()
    {
        var configs = new List<BehaviourConfig>
        {
            new(typeof(MoveBehaviour), new MoveConfig(1f, Vector3.zero, Vector3.zero)),
            new(typeof(ExplodeBehaviour), new ExplodeConfig())
        };

        var color = BlockColourResolver.Resolve(configs);

        Assert.IsTrue(ColorsAreClose(color, new Color(0.828f, 0.898f, 0.783f, 1f)));

        yield return null;
    }

    private bool ColorsAreClose(Color a, Color b, float tolerance = 0.001f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
            Mathf.Abs(a.g - b.g) < tolerance &&
            Mathf.Abs(a.b - b.b) < tolerance &&
            Mathf.Abs(a.a - b.a) < tolerance;
    }

    [UnityTest]
    public IEnumerator BlockBuilder_CreatesBlockWithConfiguredBehaviour()
    {
        var go = new GameObject();
        go.AddComponent<Block>();

        var builder = new BlockBuilder(go);
        var config = new BehaviourBuilder()
            .Add<MoveBehaviour, MoveConfig>(new MoveConfig(4f, Vector3.zero, Vector3.zero))
            .Build();

        builder.AddBehaviours(config);

        var move = go.GetComponent<MoveBehaviour>();
        Assert.IsNotNull(move);
        Assert.AreEqual(4f, move.Speed);

        yield return null;
    }

    private Block SpawnEmptyBlock(float2 position)
    {
        return blockSpawner.SpawnBlock(
            new BlockData(
                null,
                position,
                new List<BehaviourConfig>()
            )
        );
    }

}