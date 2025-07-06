using Assets.Scripts.Score;
using Assets.Scripts.SharedKernel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameHandler
{
    public class WinScene : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;
        private ISoundPlayer _soundPlayer;
        private IGameStateController _gameHandler;
        public AudioClip GetSceneMusicTheme => Resources.Load<AudioClip>("Sound/UI/Themes/win_game");
        [SerializeField] private TextMeshProUGUI _text;
        void Awake()
        {
            _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
            _gameHandler = SimpleServiceLocator.Resolve<IGameStateController>();
        }

        void Start()
        {
            _text.text += ScoreKeeper.FinalScore;
            _soundPlayer.PlaySfx(GetSceneMusicTheme);
        }

        public void LoadMainMenu()
        {
            _sceneLoader.LoadScene(SceneNames.MainMenu);
        }

        public void OnContinueClicked()
        {
            GameStateStorage.CurrentLevel++;
            _sceneLoader.LoadScene(SceneNames.Level0);
        }
    }
}