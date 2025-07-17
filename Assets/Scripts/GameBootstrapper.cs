using UnityEngine;
using Assets.Scripts.SharedKernel;
using Assets.Scripts.Blocks;
using Assets.Scripts.GameHandler;
using Assets.Scripts.Level;
using Assets.Scripts.Score;
using Assets.Scripts.HeartSystem;
using Assets.Scripts.Ball;


public class GameBootstrapper : MonoBehaviour
{
    private BlockFactory _blockFactory;
    private BlockWinConditionCounter _blockCounter;
    private SceneLoader _sceneLoader;
    private LevelDesigner _levelDesigner;
    [SerializeField] private SoundPlayer _soundPlayerPrefab;
    private SoundPlayer _soundPlayerInstance;

    [SerializeField] private GameHandler _gameHandlerPrefab;
    private ScoreTracker _scoreTracker;
    private GameHandler _gameHandlerInstance;
    private HeartController _heartController;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _pausePanelPrefab;
    [SerializeField] private GameObject _instructionsUiPrefab;
    [SerializeField] private GameObject _destructionCoordinatorPrefab;
    private ShrapnelPool _shrapnelPool;
    void Awake()
    {
        _blockFactory = GetComponent<BlockFactory>();
        _blockCounter = new BlockWinConditionCounter();
        _sceneLoader = GetComponent<SceneLoader>();
        _levelDesigner = GetComponent<LevelDesigner>();
        _scoreTracker = GetComponent<ScoreTracker>();
        _heartController = new HeartController(1);
        _shrapnelPool = GetComponent<ShrapnelPool>();


        RegisterServices();

        _gameHandlerInstance = Instantiate(_gameHandlerPrefab);
        SimpleServiceLocator.Register<IGameStateController>(_gameHandlerInstance);

        _soundPlayerInstance = Instantiate(_soundPlayerPrefab);
        SimpleServiceLocator.Register<ISoundPlayer>(_soundPlayerInstance);

        if (_sceneLoader.IsCurrentSceneLevel())
        {
            GameObject _ballInstance = Instantiate(_ballPrefab);
            IBallController ballController = _ballInstance.GetComponent<IBallController>();
            SimpleServiceLocator.Register(ballController);

            GameObject _instructionsUiInstance = Instantiate(_instructionsUiPrefab);
            InstructionsUI instructionController = _instructionsUiInstance.GetComponent<InstructionsUI>();
            SimpleServiceLocator.Register(instructionController);

            GameObject _destructionCoordinatorInstance = Instantiate(_destructionCoordinatorPrefab);
            DestructionCoordinator destructionCoordinator = _destructionCoordinatorInstance.GetComponent<DestructionCoordinator>();
            SimpleServiceLocator.Register(destructionCoordinator);
        }

        GameObject pausePanelInstance = Instantiate(_pausePanelPrefab);
        IPauseController pauseController = pausePanelInstance.GetComponent<IPauseController>();
        SimpleServiceLocator.Register(pauseController);
    }

    private void RegisterServices()
    {
        SimpleServiceLocator.Register<IBlockFactory>(_blockFactory);
        SimpleServiceLocator.Register<IBlockCounter>(_blockCounter);
        SimpleServiceLocator.Register<IGameWinCondition>(_blockCounter);
        SimpleServiceLocator.Register<ISceneLoader>(_sceneLoader);
        SimpleServiceLocator.Register<ILevelDesigner>(_levelDesigner);
        SimpleServiceLocator.Register<IScoreTracker>(_scoreTracker);
        SimpleServiceLocator.Register<IHeartController>(_heartController);
        SimpleServiceLocator.Register<ILevelCatalog>(new StaticLevelCatalog());
        SimpleServiceLocator.Register<IShrapnelPool>(_shrapnelPool);
    }
}
