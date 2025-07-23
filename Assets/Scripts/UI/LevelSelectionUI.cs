using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameHandler;
using Assets.Scripts.Level;
using Assets.Scripts.SharedKernel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _scoreNumber;
        private ILevelCatalog _levelCatalog;
        private ISceneLoader _sceneLoader;
        private LevelProgressService _levelProgressService;
        private List<LevelDefinition> _levels;
        private int _currentIndex = 0;
        [SerializeField] private GameObject _currentButton;
        public int GetCurrentlySelectedLevelId() => _levels[_currentIndex].Id;


        void Awake()
        {
            _levelCatalog = SimpleServiceLocator.Resolve<ILevelCatalog>();
            _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
            _levels = _levelCatalog.GetAvailableLevels().ToList();
            _levelProgressService = SimpleServiceLocator.Resolve<LevelProgressService>();

            _prevButton.onClick.AddListener(ShowPrevious);
            _nextButton.onClick.AddListener(ShowNext);
        }

        void Start()
        {
            RenderCurrent();
        }

        public void ShowPrevious()
        {
            _currentIndex = (_currentIndex - 1 + _levels.Count) % _levels.Count;
            RenderCurrent();
        }

        public void ShowNext()
        {
            _currentIndex = (_currentIndex + 1) % _levels.Count;
            RenderCurrent();
        }

        private void RenderCurrent()
        {
            var level = _levels[_currentIndex];
            bool isUnlocked = _levelProgressService.IsUnlocked(level.Id.ToString());

            _currentButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{_currentIndex}. {level.DisplayName}";
            _description.text = isUnlocked ? level.Description : "Level locked";

            int score = _levelProgressService.GetScore(level.Id.ToString());
            _scoreNumber.text = isUnlocked ? score.ToString() : "N/A";

            var button = _currentButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            if (isUnlocked)
                button.onClick.AddListener(() => LoadLevel(level.Id));
            else
                button.onClick.AddListener(() => Debug.Log("Level is locked"));
        }
        private void LoadLevel(int levelId)
        {
            GameStateStorage.CurrentLevel = levelId;
            _sceneLoader.LoadScene(SceneNames.Level0, levelId);
        }
    }   
}