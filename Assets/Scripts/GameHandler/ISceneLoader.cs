namespace Assets.Scripts.GameHandler
{
    public interface ISceneLoader
    {
        public void LoadScene(string sceneName, int levelIndex = 1);
        public bool IsCurrentSceneLevel();
    }
}