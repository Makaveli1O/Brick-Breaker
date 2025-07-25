using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.GameHandler
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        private MainMenuScene _mainMenuScene;
        [SerializeField] private LevelSelectionUI _levelSelectionUI;


        private int _selectedIndex = 0;
        private readonly Color _normalColor = Color.white;
        private readonly Color _highlightColor = Colours.ButtonTextColorHover;

        private void Start()
        {
            _mainMenuScene = GetComponent<MainMenuScene>();
            HighlightSelected();

            for (int i = 0; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[i].onClick.AddListener(() => ConfirmSelectionFromButton(index));
            }
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
            _selectedIndex = (_selectedIndex + direction + _buttons.Length) % _buttons.Length;
            HighlightSelected();
        }

        private void HighlightSelected()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                var text = _buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                text.color = (i == _selectedIndex) ? _highlightColor : _normalColor;
            }
        }

        private void ConfirmSelection(int selectedLevelId)
        {
            switch (_selectedIndex)
            {
                case 0:
                    if (_levelSelectionUI.IsCurrentlySelectedLevelUnlocked())
                        _mainMenuScene.PlayGame(selectedLevelId);
                    else
                        Debug.Log("Selected level is locked");
                    break;
                case 1:
                    _mainMenuScene.ToggleInstructions();
                    break;
                case 2:
                    _mainMenuScene.ExitGame();
                    break;
            }
        }

        private void ConfirmSelectionFromButton(int index)
        {
            _selectedIndex = index;
            ConfirmSelection(_levelSelectionUI.GetCurrentlySelectedLevelId());
        }
    }
}
