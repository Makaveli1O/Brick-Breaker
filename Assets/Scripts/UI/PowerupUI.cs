using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PowerupUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Image _icon;
        [SerializeField] private Slider _timer;

        private float _duration;
        private float _remaining;

        private bool _active;

        public void Show(float duration)
        {
            _container.SetActive(true);
            _duration = duration;
            _remaining = duration;
            _active = true;
        }

        public void Hide()
        {
            _container.SetActive(false);
            _active = false;
        }

        private void Update()
        {
            if (!_active) return;

            _remaining -= Time.deltaTime;
            _timer.value = Mathf.Clamp01(_remaining / _duration);

            if (_remaining <= 0f)
                Hide();
        }
    }
}
