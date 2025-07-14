namespace Assets.Scripts.Level
{
    public interface ILevelDesigner
    {
        void LoadLevel(LevelData levelData);
        void LoadLevel(int levelId);
    }
}