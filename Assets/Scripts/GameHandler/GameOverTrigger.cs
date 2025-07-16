using UnityEngine;
using Assets.Scripts.SharedKernel;
using Assets.Scripts.HeartSystem;

namespace Assets.Scripts.GameHandler{
    public class GameOverTrigger : MonoBehaviour
    {
        private string GetSceneName => SceneNames.GameOver;
        private ISceneLoader _sceneLoader;
        private IHeartController _heartController;

        void Awake()
        {
            _sceneLoader = SimpleServiceLocator.Resolve<ISceneLoader>();
            _heartController = SimpleServiceLocator.Resolve<IHeartController>();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Ball"))
                return;

            _heartController.RemoveHeart();

            if (_heartController.GetCurrentHeart <= 0)
                _sceneLoader.LoadScene(GetSceneName, GameStateStorage.CurrentLevel); 
        }
    }
}