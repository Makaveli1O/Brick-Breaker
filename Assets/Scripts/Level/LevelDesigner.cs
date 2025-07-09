using Assets.Scripts.Blocks;
using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelDesigner : MonoBehaviour, ILevelDesigner
    {
        private BlockSpawner _spawner;
        private ISoundPlayer _soundPlayer;
        public AudioClip GetSceneMusicTheme => Resources.Load<AudioClip>("Sound/UI/Themes/game_loop");
        private IGameStateController _gameStateController;


        void Awake()
        {
            _spawner = GetComponent<BlockSpawner>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _gameStateController = SimpleServiceLocator.Resolve<IGameStateController>();
        }

        void Start()
        {   
            LoadLevel(GetLevelData(GameStateStorage.CurrentLevel));
            _gameStateController.SetState(GameState.Loaded);
            _soundPlayer.PlayMusic(GetSceneMusicTheme);
        }

        public LevelData GetLevelData(int levelIndex)
        {
            return levelIndex switch
            {
                1 => GetLevel3(),
                2 => GetLevel2(),
                3 => GetLevel3(),
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

            // Outer slow movers (top & bottom rows)
            for (int x = -4; x <= 4; x += 2)
            {
                builder.WithBlock(new int2(x, 4), slowMover);
                builder.WithBlock(new int2(x, -4), slowMover);
            }

            // Middle stationary exploders (sides)
            for (int y = -2; y <= 2; y += 2)
            {
                builder.WithBlock(new int2(-4, y), stationaryExploder);
                builder.WithBlock(new int2(4, y), stationaryExploder);
            }

            // Inner fast combined
            builder.WithBlock(new int2(0, 0), fastCombo);
            builder.WithBlock(new int2(-2, 0), fastCombo);
            builder.WithBlock(new int2(2, 0), fastCombo);

            return builder.Build();
        }

        private LevelData GetLevel2()
        {
            var diagonalMover = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(2.5f, new Vector3(-3, -3, 0), new Vector3(3, 3, 0))
                )
                .Build();

            var verticalMover = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(3f, new Vector3(0, -4, 0), new Vector3(0, 4, 0))
                )
                .Build();

            var stationaryExploder = new BehaviourBuilder()
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var comboFast = new BehaviourBuilder()
                .Add<MoveBehaviour, MoveConfig>(
                    new MoveConfig(4.5f, new Vector3(-2, 0, 0), new Vector3(2, 0, 0))
                )
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            var builder = new LevelBuilder();

            // Diagonal arms
            builder.WithBlock(new int2(-3, -3), diagonalMover);
            builder.WithBlock(new int2(3, 3), diagonalMover);
            builder.WithBlock(new int2(-3, 3), diagonalMover);
            builder.WithBlock(new int2(3, -3), diagonalMover);

            // Center fast combo
            builder.WithBlock(new int2(0, 0), comboFast);

            // Vertical cross (exploders)
            builder.WithBlock(new int2(0, 2), stationaryExploder);
            builder.WithBlock(new int2(0, -2), stationaryExploder);

            // Horizontal cross (movers)
            builder.WithBlock(new int2(-2, 0), verticalMover);
            builder.WithBlock(new int2(2, 0), verticalMover);

            return builder.Build();
        }

        // TODO Test level for explosion behaviour performance
        private LevelData GetLevel3()
        {
            var builder = new LevelBuilder();

            var exploder = new BehaviourBuilder()
                .AddNonConfigurable<ExplodeBehaviour>()
                .Build();

            float s = BlockGrid.Spacing;

            for (int x = -10; x <= 10; x++)
            {
                for (int y = -10; y <= 10; y++)
                {
                    builder.WithBlock(new float2(x * s, y * s), exploder);
                }
            }

            return builder.Build();
        }


        public void LoadLevel(LevelData levelData)
        {
            foreach (BlockData data in levelData.Blocks)
                _spawner.SpawnBlock(data);
        }
    }
}
