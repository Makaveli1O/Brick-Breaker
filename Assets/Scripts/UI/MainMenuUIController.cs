using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem; // ✅ New Input System namespace

namespace Assets.Scripts.GameHandler
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _options;
        private MainMenuScene _mainMenuScene;
        [SerializeField] private LevelSelectionUI _levelSelectionUI;


        private int _selectedIndex = 0;
        private readonly Color _normalColor = Color.white;
        private readonly Color _highlightColor = Color.yellow;

        private void Start()
        {
            _mainMenuScene = GetComponent<MainMenuScene>();
            HighlightSelected();
        }

        private void Update()
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
                MoveSelection(-1);
            else if (Keyboard.current.sKey.wasPressedThisFrame)
                MoveSelection(1);
            else if (Keyboard.current.enterKey.wasPressedThisFrame)
                ConfirmSelection(_levelSelectionUI.GetCurrentlySelectedLevelId());
            else if (Keyboard.current.aKey.wasPressedThisFrame)
                _levelSelectionUI.ShowPrevious();
            else if (Keyboard.current.dKey.wasPressedThisFrame)
                _levelSelectionUI.ShowNext();
        }

        private void MoveSelection(int direction)
        {
            _selectedIndex = (_selectedIndex + direction + _options.Length) % _options.Length;
            HighlightSelected();
        }

        private void HighlightSelected()
        {
            for (int i = 0; i < _options.Length; i++)
                _options[i].color = (i == _selectedIndex) ? _highlightColor : _normalColor;
        }

        private void ConfirmSelection(int selectedLevelId)
        {
            switch (_selectedIndex)
            {
                case 0: _mainMenuScene.PlayGame(selectedLevelId); break;
                case 1: _mainMenuScene.ToggleInstructions(); break;
                case 2: _mainMenuScene.ExitGame(); break;
            }
        }
    }
}
