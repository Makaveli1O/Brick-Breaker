using Assets.Scripts.GameHandler;
using Assets.Scripts.SharedKernel;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private ISceneLoader _sceneLoader;
    private ISoundPlayer _soundPlayer;
    public AudioClip GetSceneMusicTheme => Resources.Load<AudioClip>("Sound/UI/Themes/over_game");

    void Awake()
    {
        _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
        _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
    }

    void Start()
    {
        _soundPlayer.PlaySfx(GetSceneMusicTheme);
    }

    public void BackToMenu()
    {
        _sceneLoader.LoadScene(SceneNames.MainMenu, GameStateStorage.CurrentLevel);
    }
}
