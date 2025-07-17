using NUnit.Framework;
using UnityEngine;
using Assets.Scripts.Blocks;
using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework.Constraints;
using Assets.Scripts.Score;

public class GameHandlerTest
{
    private GameObject _gameHandlerGO;
    private GameHandler _gameHandler;
    private StubSceneLoader _sceneLoader;
    private StubWinCondition _winCondition;
    private ScoreTracker _scoreTracker;
    private LevelProgressService _levelProgressService;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _sceneLoader = new StubSceneLoader();
        _winCondition = new StubWinCondition();
        _scoreTracker = new();
        _levelProgressService = new LevelProgressService(new LevelProgressRepositoryMock());

        SimpleServiceLocator.Clear();
        SimpleServiceLocator.Register<IScoreTracker>(_scoreTracker);
        SimpleServiceLocator.Register<ISceneLoader>(_sceneLoader);
        SimpleServiceLocator.Register<IGameWinCondition>(_winCondition);
        SimpleServiceLocator.Register<LevelProgressService>(_levelProgressService);

        _gameHandlerGO = new GameObject("GameHandler");
        _gameHandler = _gameHandlerGO.AddComponent<GameHandler>();

        yield return null; // Allow Monobehaviour lifecycle
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(_gameHandlerGO);
        yield return null;


        SimpleServiceLocator.Clear(); // MUST happen always
    }

    [UnityTest]
    public IEnumerator Should_TransitionToWin_WhenWinConditionMet()
    {
        _gameHandler.SetState(GameState.Playing);
        _winCondition.Result = true;

        yield return null; // allow Update()

        Assert.AreEqual(GameState.Win, GetCurrentState(_gameHandler));
        Assert.AreEqual(SceneNames.Win, _sceneLoader.LastLoadedScene);
    }

    [UnityTest]
    public IEnumerator ShouldPauseTime_WhenTransitionToPaused()
    {
        _gameHandler.SetState(GameState.Paused);
        yield return null;
        Assert.AreEqual(0f, Time.timeScale);
    }

    [UnityTest]
    public IEnumerator ShouldLoadGameOverScene_WhenStateIsGameOver()
    {
        _gameHandler.SetState(GameState.GameOver);
        yield return null;

        Assert.AreEqual(SceneNames.GameOver, _sceneLoader.LastLoadedScene);
        Assert.AreEqual(GameState.GameOver, GetCurrentState(_gameHandler));
    }

    [UnityTest]
    public IEnumerator ShouldNotTriggerWin_IfNotInPlayingState()
    {
        _gameHandler.SetState(GameState.Paused);
        _winCondition.Result = true;

        yield return null;

        Assert.AreEqual(GameState.Paused, GetCurrentState(_gameHandler));
        Assert.IsNull(_sceneLoader.LastLoadedScene);
    }

    private GameState GetCurrentState(GameHandler handler)
    {
        return (GameState)typeof(GameHandler)
            .GetField("_currentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(handler);
    }
}