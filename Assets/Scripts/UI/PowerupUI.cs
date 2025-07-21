using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PowerupUI : MonoBehaviour
    {
        private float _breatheDuration = 1f; //Duration of single breath
        private float _totalDisplayDuration = 3f;
        private float _breathePhaseDuration = 3f;

        private CanvasGroup _canvasGroup;
        private float _timer;
        private bool _initialized;

        public float BreathePhaseStart => _totalDisplayDuration - _breathePhaseDuration;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1f;
        }

        public void Initialize(float duration)
        {
            _totalDisplayDuration = duration;
            _initialized = true;
        }

        void Update()
        {
            if (!_initialized) return;

            _timer += Time.deltaTime;

            if (_timer < BreathePhaseStart)
            {
                _canvasGroup.alpha = 1f;
            }
            else if (_timer < _totalDisplayDuration)
            {
                ApplyBreathing();
            }
        }

        private void ApplyBreathing()
        {
            float localTime = _timer - BreathePhaseStart;
            float pulse = (Mathf.Sin(localTime * Mathf.PI * 2 / _breatheDuration) + 1f) / 2f;
            _canvasGroup.alpha = Mathf.Lerp(0.2f, 1f, pulse);
        }
    }
}
