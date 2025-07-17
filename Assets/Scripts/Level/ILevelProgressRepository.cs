namespace Assets.Scripts.Level
{
    public interface ILevelProgressRepository
    {
        LevelProgressData GetProgress(string levelId);
        void SaveProgress(LevelProgressData data);
        bool IsLevelUnlocked(string levelId);
        void UnlockLevel(string levelId);
    }
}
