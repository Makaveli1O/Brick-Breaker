using Assets.Scripts.SharedKernel;

public class LevelProgressRepositoryMock : ILevelProgressRepository
{
    public const string _testLevelId = "1";
    public const int _testLevelScore = 111;
    public bool LevelUnlockedStub { get; set; } = true;
    public LevelProgressData GetProgress(string levelId)
    {
        return new LevelProgressData(_testLevelId, _testLevelScore);
    }

    public bool IsLevelUnlocked(string levelId) => LevelUnlockedStub;
    public void SaveProgress(LevelProgressData data)
    {
        return;
    }

    public void UnlockLevel(string levelId)
    {
        return;
    }
}