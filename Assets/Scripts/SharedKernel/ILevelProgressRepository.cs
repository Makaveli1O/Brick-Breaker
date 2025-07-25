namespace Assets.Scripts.SharedKernel
{
    public interface ILevelProgressRepository
    {
        LevelProgressData GetProgress(string levelId);
        void SaveProgress(LevelProgressData data);
        bool IsLevelUnlocked(string levelId);
        void UnlockLevel(string levelId);
        void ResetProgress();
    }
}
