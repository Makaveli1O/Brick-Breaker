using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameHandler
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        public string GetCurrentSceneName => SceneManager.GetActiveScene().name;
        public bool IsCurrentSceneLevel() => GetCurrentSceneName.Contains("Level");
        public void LoadScene(string sceneName, int levelIndex)
        {
            GameStateStorage.CurrentLevel = levelIndex;
            SceneManager.LoadScene(sceneName);
        } 
    }
}