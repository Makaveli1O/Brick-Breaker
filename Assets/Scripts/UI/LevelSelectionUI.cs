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
        private ILevelCatalog _levelCatalog;
        private ISceneLoader _sceneLoader;
        private List<LevelDefinition> _levels;
        private int _currentIndex = 0;
        [SerializeField] private GameObject _currentButton;

        void Awake()
        {
            _levelCatalog = SimpleServiceLocator.Resolve<ILevelCatalog>();
            _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
            _levels = _levelCatalog.GetAvailableLevels().ToList();

            _prevButton.onClick.AddListener(ShowPrevious);
            _nextButton.onClick.AddListener(ShowNext);
        }

        void Start()
        {
            RenderCurrent();
        }

        private void ShowPrevious()
        {
            _currentIndex = (_currentIndex - 1 + _levels.Count) % _levels.Count;
            RenderCurrent();
        }

        private void ShowNext()
        {
            _currentIndex = (_currentIndex + 1) % _levels.Count;
            RenderCurrent();
        }

        private void RenderCurrent()
        {
            var level = _levels[_currentIndex];

            _currentButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{_currentIndex}. {level.DisplayName}";
            _description.text = level.Description ?? "";
            _currentButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _currentButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level.Id));
        }

        private void LoadLevel(int levelId)
        {
            GameStateStorage.CurrentLevel = levelId;
            _sceneLoader.LoadScene(SceneNames.Level0, levelId);
        }
    }   
}