using System.Collections.Generic;
using Assets.Scripts.Ball;
using Assets.Scripts.Blocks;
using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using Assets.Scripts.UI;
using Unity.Mathematics;
using UnityEngine;
using Assets.Scripts.Level.Config;
using Assets.Scripts.HeartSystem;

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
        private InstructionsUI _instructionsUI;
        private GridSystem _grid;
        private IBallController _ballController;
        private IHeartController _heartController;


        void Awake()
        {
            _spawner = GetComponent<BlockSpawner>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _gameStateController = SimpleServiceLocator.Resolve<IGameStateController>();
            _instructionsUI = SimpleServiceLocator.Resolve<InstructionsUI>();
            _ballController = SimpleServiceLocator.Resolve<IBallController>();
            _heartController = SimpleServiceLocator.Resolve<IHeartController>();
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

        // TODO make levels to gradually introduc eplayer to the mechanics
        // movement W/S, launch ball with F
        // Showcase HP system
        // Showcase individual blocks, their relationship to the colours
        // and their possibility to combine their behaviour.
        private LevelData GetLevel1()
        {
            var basicBlock = new BehaviourBuilder()
            .AddNonConfigurable<BasicBehaviour>()
            .Build();

            Vector2 launchHorizontalLeft = Vector2.left;

            var builder = new LevelBuilder()
                .WithBlock(-3, -1, basicBlock, _grid)
                .WithConfig(LevelConfigFactory.WithBall(launchDirection: Vector2.left));

            _instructionsUI.SetText("Use 'W' and 'S' to move.\nPress 'F' to launch the ball.");
            return builder.Build();
        }
        
        private LevelData GetLevel2()
        {
            var basicBlock = new BehaviourBuilder()
                .AddNonConfigurable<BasicBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            for (int y = 0; y <= 4; y++)
            {
                builder.WithBlock(-2, y, basicBlock, _grid);
            }

            builder.WithBlock(2, 4, basicBlock, _grid);

            builder.WithConfig(LevelConfigFactory.WithBall(launchDirection:Vector2.left));

            _instructionsUI.SetText(
                "There are several paddles with different \n" +
                "behaviours. This can rotate in 45 degrees. \n" +
                "Use 'A' and 'D' to rotate the paddle.\n" +
                "Find an angle to shoot around the wall.\n" +
                "Press 'F' to launch the ball."
            );

            return builder.Build();
        }


        private LevelData GetLevel3()
        {
            var basicBlock = new BehaviourBuilder()
                .AddNonConfigurable<BasicBehaviour>()
                .Build();

            Vector2 launchDiagonal = new Vector2(-1, 1).normalized;

            var builder = new LevelBuilder()
                .WithBlock(3, 3, basicBlock, _grid)
                .WithBlock(4, 3, basicBlock, _grid)
                .WithBlock(5, 3, basicBlock, _grid)
                .WithBlock(6, 3, basicBlock, _grid)
                .WithBlock(7, 3, basicBlock, _grid)
                .WithConfig(
                    LevelConfigFactory.WithBall(launchDirection: launchDiagonal, initialPush: 400f)
                );

            _instructionsUI.SetText("Press 'SPACEBAR' to temporarily boost paddle speed.\nTry catching the ball launched sideways!");
            return builder.Build();
        }

        private LevelData GetLevel4()
        {
            var basicBlock = new BehaviourBuilder()
                .AddNonConfigurable<BasicBehaviour>()
                .Build();

            var builder = new LevelBuilder()
                .WithBlock(-3, 2, basicBlock, _grid)
                .WithConfig(
                    LevelConfigFactory.WithBall(
                        launchDirection: new Vector2(0, -1), // downward launch
                        position: new Vector2(0, 4),         // ball starts above paddle
                        initialPush: 1f                      // ensure it falls fast
                    ))
                .WithConfig(LevelConfigFactory.WithHP(2));

            _instructionsUI.SetText(
                "This time, don’t catch the ball.\n" +
                "Let it fall to see what happens when you miss.\n" +
                "Each miss costs one heart."
            );

            return builder.Build();
        }

        private LevelData GetLevel5()
        {
            var reflectMover = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(1.5f, new Vector3(-3, 2, 0), new Vector3(3, 2, 0))
                )
                .AddNonConfigurable<ReflectBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            for (int x = -1; x <= 1; x++)
            {
                builder.WithBlock(x, 2, reflectMover, _grid);
            }

            _instructionsUI.SetText("These reflectors are on the move.");
            return builder.Build();
        }

        private LevelData GetLevel6()
        {
            var movingExploder = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(2f, new Vector3(-4, 3, 0), new Vector3(4, 3, 0))
                )
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var reflectBlock = new BehaviourBuilder()
                .AddNonConfigurable<ReflectBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            builder.WithBlock(0, 4, movingExploder, _grid);
            builder.WithBlock(-2, 2, reflectBlock, _grid);
            builder.WithBlock(2, 2, reflectBlock, _grid);

            _instructionsUI.SetText("Everything combined: reflect, explode, and move!");
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

            ApplyBallConfig(levelData.LevelConfig.BallConfig);

            _heartController.SetMaxHearts(levelData.LevelConfig.InitialHP);
        }

        private void ApplyBallConfig(BallConfig config)
        {
            _ballController.SetLaunchDirection(config.LaunchDirection);
            _ballController.SetInitialPosition(config.BallInitialPosition);
            _ballController.SetInitialPush(config.BallInitialPush);
        }
    }
}
