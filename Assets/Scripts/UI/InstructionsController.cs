using UnityEngine;
using Assets.Scripts.Level;
using Assets.Scripts.UI;

namespace Assets.Scripts.GameHandler
{
    public class InstructionsController : MonoBehaviour
    {
        [SerializeField] private InstructionsUI _instructionsUI;

        public void ShowForLevel(int levelIndex)
        {
            string message = levelIndex switch
            {
                1 => "Exploding blocks ahead. Avoid the blast!",
                2 => "Reflecting blocks. Watch your angles.",
                3 => "Exploding blocks in a wall formation.",
                4 => "Reflector line and a trigger block.",
                5 => "Slowing blocks. Timing is key.",
                6 => "Vertical reflectors and a slow block.",
                7 => "Combo blocks: slow + explode. Handle carefully.",
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(message))
            {
                _instructionsUI.SetText(message);
                _instructionsUI.Show();
            }
            else
            {
                _instructionsUI.Hide();
            }
        }

        public void Hide()
        {
            _instructionsUI.Hide();
        }
    }
}
