using Assets.Scripts.Level.Config;

namespace Assets.Scripts.Level
{
    public interface ILevelDesigner
    {
        LevelData GetLevelData(int levelIndex);
        void LoadLevel(LevelData levelData);
        void LoadLevel(int levelId);
    }
}