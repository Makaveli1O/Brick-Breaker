using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.GameHandler
{
    public class MainMenuScene : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;
        private ISoundPlayer _soundPlayer;
        private string GetInitialSceneName => SceneNames.Level0;
        public AudioClip GetSceneMusicTheme => Resources.Load<AudioClip>("Sound/UI/Themes/main_menu");

        void Awake()
        {
            _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
        }

        void Start()
        {
            _soundPlayer.PlayMusic(GetSceneMusicTheme);
        }

        public void PlayGame()
        {
            _sceneLoader.LoadScene(GetInitialSceneName, 1);
        }

        public void ContinueGame()
        {
            _sceneLoader.LoadScene(GetInitialSceneName, GameStateStorage.CurrentLevel);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }

}