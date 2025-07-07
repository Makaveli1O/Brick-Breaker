using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using UnityEngine;

public class PauseController : MonoBehaviour, IPauseController
{
    private IGameStateController _gameStateController;
    private ISceneLoader _sceneLoader;

    void Awake()
    {
        _gameStateController = SimpleServiceLocator.Resolve<IGameStateController>();
        _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
    }

    public void OnContinueClicked()
    {
        gameObject.SetActive(false);
        _gameStateController.SetState(GameState.Playing);
    }

    public void OnExitClicked()
    {
        Time.timeScale = 1f;
        _sceneLoader.LoadScene(SceneNames.MainMenu);
    }

    public void TogglePause()
    {
        if (SimpleServiceLocator.Resolve<IGameStateController>().CurrentState == GameState.Paused)
        {
            gameObject.SetActive(false); 
            SimpleServiceLocator.Resolve<IGameStateController>().SetState(GameState.Playing);
        }
        else
        {
            gameObject.SetActive(true);
            SimpleServiceLocator.Resolve<IGameStateController>().SetState(GameState.Paused);
        }
    }

    public void Pause()
    {
        gameObject.SetActive(false); // not working
        SimpleServiceLocator.Resolve<IGameStateController>().SetState(GameState.Paused);
    }

    public void UnPause()
    {
        gameObject.SetActive(false);
        _gameStateController.SetState(GameState.Playing);
    }
}
