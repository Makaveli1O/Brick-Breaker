using Assets.Scripts.GameHandler;

public class StubSceneLoader : ISceneLoader
{
    public string LastLoadedScene;
    public void LoadScene(string sceneName) => LastLoadedScene = sceneName;
    public bool IsCurrentSceneLevel() => false;
}