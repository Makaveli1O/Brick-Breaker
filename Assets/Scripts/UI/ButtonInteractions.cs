using Assets.Scripts.SharedKernel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class ButtonInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI _buttonText;
        private ISoundPlayer _soundPlayer;
        [SerializeField] private AudioClip _hoverSound;

        void Awake()
        {
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonText.color = Colours.ButtonTextColorHover;
            _soundPlayer.PlaySfx(_hoverSound);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonText.color = Colours.ButtonTextColor;
        }
    }
}
