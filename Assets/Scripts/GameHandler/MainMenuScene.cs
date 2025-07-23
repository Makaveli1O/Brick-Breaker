using Assets.Scripts.SharedKernel;
using UnityEngine;

namespace Assets.Scripts.GameHandler
{
    public class MainMenuScene : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;
        private ISoundPlayer _soundPlayer;
        [SerializeField] private GameObject _instructions;
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

        public void ToggleInstructions()
        {
            _instructions.SetActive(!_instructions.activeSelf);
        }

        public void PlayGame(int levelId)
        {
            _sceneLoader.LoadScene(GetInitialSceneName, levelId);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }

}