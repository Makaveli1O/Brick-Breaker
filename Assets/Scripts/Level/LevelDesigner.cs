using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelDesigner : MonoBehaviour, ILevelDesigner
    {
        private const float _xStepSize = 0.2f;
        private const float _yStepSize = 0.3f;
        private BlockSpawner _spawner;
        private ISoundPlayer _soundPlayer;
        public AudioClip GetSceneMusicTheme => Resources.Load<AudioClip>("Sound/UI/Themes/game_loop");
        private IGameStateController _gameStateController;
        private GridSystem _grid;


        void Awake()
        {
            _spawner = GetComponent<BlockSpawner>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _gameStateController = SimpleServiceLocator.Resolve<IGameStateController>();
        }

        void Start()
        {
            _grid = new GridSystem(_spawner.GetBlockSizeInWorldUnits().x, _spawner.GetBlockSizeInWorldUnits().y);
            LoadLevel(GetLevelData(GameStateStorage.CurrentLevel));
            _gameStateController.SetState(GameState.Loaded);
            // _soundPlayer.PlayMusic(GetSceneMusicTheme);
            _soundPlayer.PlayMusic(null);
        }

        public LevelData GetLevelData(int levelIndex)
        {
            return levelIndex switch
            {
                1 => GetLevel1(),
                2 => GetLevel2(),
                3 => GetLevel3(),
                4 => GetLevel4(),
                5 => GetLevel5(),
                6 => GetLevel6(),
                7 => GetLevel7(),
                _ => GetLevel1()
            };
        }

        private LevelData GetLevel1()
        {
            var slowMover = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(1.0f, new Vector3(-4, 4, 0), new Vector3(4, 4, 0))
                )
                .Build();

            var stationaryExploder = new BehaviourBuilder()
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var fastCombo = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(4.0f, new Vector3(-2, 0, 0), new Vector3(2, 0, 0))
                )
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            for (int y = -2; y <= 2; y++)
            {
                builder.WithBlock(-8, y, stationaryExploder, _grid);
            }

            return builder.Build();
        }

        
        private LevelData GetLevel2()
        {
            var reflecter = new BehaviourBuilder()
                .Add<ReflectBehaviour, ReflectConfig>(
                    new ReflectConfig(Vector2.left)
                )
                .Build();

            var builder = new LevelBuilder();

            builder.WithBlock(3, -3, reflecter, _grid);

            return builder.Build();
        }

        private LevelData GetLevel3()
        {
            var builder = new LevelBuilder();

            var exploder = new BehaviourBuilder()
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            for (int x = -10; x <= -7; x++)
            {
                for (int y = -3; y <= 3; y++)
                {
                    builder.WithBlock(x, y, exploder, _grid);
                }
            }

            return builder.Build();
        }

        private LevelData GetLevel4()
        {
            var reflecter = new BehaviourBuilder()
                .Add<ReflectBehaviour, ReflectConfig>(
                    new ReflectConfig(Vector2.left)
                )
                .Build();

            var exploder = new BehaviourBuilder()
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            for (int y = -4; y <= 0; y++)
                builder.WithBlock(4, y, reflecter, _grid);

            builder.WithBlock(3, 0, exploder, _grid);

            return builder.Build();
        }

        private LevelData GetLevel5()
        {
            var slower = new BehaviourBuilder()
                .Add<SlowBehaviour, SlowConfig>(
                    new SlowConfig(2f, 0.5f)
                )
                .Build();

            var builder = new LevelBuilder();

            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    builder.WithBlock(x, y, slower, _grid);
                }
            }

            return builder.Build();
        }

        private LevelData GetLevel6()
        {
            var slower = new BehaviourBuilder()
                .Add<SlowBehaviour, SlowConfig>(
                    new SlowConfig(2f, 0.3f)
                )
                .Build();

            var reflector = new BehaviourBuilder()
                .Add<ReflectBehaviour, ReflectConfig>(
                    new ReflectConfig(Vector2.up)
                )
                .Build();

            var builder = new LevelBuilder();

            for (int y = -3; y <= 3; y++)
                builder.WithBlock(3, y, reflector, _grid);

            builder.WithBlock(1, 0, slower, _grid);

            return builder.Build();
        }

        private LevelData GetLevel7()
        {
            var combo = new BehaviourBuilder()
                .Add<SlowBehaviour, SlowConfig>(
                    new SlowConfig(1.5f, 0.4f)
                )
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    builder.WithBlock(x, y, combo, _grid);
                }
            }

            return builder.Build();
        }

        private IEnumerable<float2> GenerateYCoords(float start, float end, float xPos)
        {
            float step = Mathf.Sign(end - start) * Mathf.Abs(_yStepSize);
            for (float y = start; step > 0 ? y <= end : y >= end; y += step)
                yield return new float2(xPos, y);
        }

        private IEnumerable<float2> GenerateXCoords(float start, float end, float yPos)
        {
            float step = Mathf.Sign(end - start) * Mathf.Abs(_xStepSize);
            for (float x = start; step > 0 ? x <= end : x >= end; x += step)
                yield return new float2(x, yPos);
        }

        private IEnumerable<float2> GenerateGrid(float xStart, float xEnd, float xStep, float yStart, float yEnd, float yStep)
        {
            float xDir = Mathf.Sign(xEnd - xStart) * Mathf.Abs(xStep);
            float yDir = Mathf.Sign(yEnd - yStart) * Mathf.Abs(yStep);

            for (float x = xStart; xDir > 0 ? x <= xEnd : x >= xEnd; x += xDir)
                for (float y = yStart; yDir > 0 ? y <= yEnd : y >= yEnd; y += yDir)
                    yield return new float2(x, y);
        }

        public void LoadLevel(int levelId)
        {
            LoadLevel(GetLevelData(levelId));
        }

        public void LoadLevel(LevelData levelData)
        {
            foreach (BlockData data in levelData.Blocks)
                _spawner.SpawnBlock(data);
        }
    }
}
