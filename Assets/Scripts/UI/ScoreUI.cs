using Assets.Scripts.Score;
using Assets.Scripts.SharedKernel;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        private IScoreTracker _scoreTracker;

        void Awake()
        {
            _scoreTracker = SimpleServiceLocator.Resolve<IScoreTracker>();
        }

        void Update()
        {
            _scoreText.text = $"Score: {_scoreTracker.CurrentScore}";
        }
    }
}