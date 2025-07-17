using System;
using Assets.Scripts.Blocks;
using Assets.Scripts.SharedKernel;
using Assets.Scripts.Score;
using UnityEngine;

namespace Assets.Scripts.GameHandler
{
    public class GameHandler : MonoBehaviour, IGameStateController
    {
        private string _winScene => SceneNames.Win;
        private string _gameOverScene => SceneNames.GameOver;
        public GameState CurrentState => _currentState;
        private IGameWinCondition _winCondition;
        private GameState _currentState;
        private ISceneLoader _sceneLoader;
        private IScoreTracker _scoreTracker;
        private LevelProgressService _levelProgressService;
        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            _winCondition = SimpleServiceLocator.Resolve<IGameWinCondition>();
            _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
            _scoreTracker = SimpleServiceLocator.Resolve<IScoreTracker>();
            _levelProgressService = SimpleServiceLocator.Resolve<LevelProgressService>();
        }

        private void Start()
        {
            if (_sceneLoader.IsCurrentSceneLevel()) _scoreTracker.StartTracking();
            SetState(GameState.Playing);
        }

        private void Update()
        {
            if (_currentState == GameState.Playing && _winCondition.IsWinConditionMet())
            {
                if (_scoreTracker.IsTrackingEnabled)
                {
                    _scoreTracker.StopTracking();
                    ScoreKeeper.FinalScore = _scoreTracker.GetFinalScore();
                }
                SetState(GameState.Win);
            }
        }

        public void SetState(GameState newState)
        {
            if (_currentState == newState)
                return;

            _currentState = newState;
            HandleStateChange(newState);
        }

        private void HandleStateChange(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    _sceneLoader.LoadScene(_gameOverScene, GameStateStorage.CurrentLevel);
                    break;
                case GameState.Win:
                    Time.timeScale = 0f;
                    SaveProgressAndLoadWinScene();
                    break;
            }
        }

        private void SaveProgressAndLoadWinScene()
        {
            string currentLevelId = GameStateStorage.CurrentLevel.ToString();
            _levelProgressService.CompleteLevel(
                currentLevelId,
                _scoreTracker.GetFinalScore(),
                (GameStateStorage.CurrentLevel + 1).ToString()
            );

            _sceneLoader.LoadScene(_winScene, GameStateStorage.CurrentLevel++);
        }
    }
}
