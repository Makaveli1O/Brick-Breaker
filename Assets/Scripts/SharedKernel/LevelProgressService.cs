namespace Assets.Scripts.SharedKernel
{
    public class LevelProgressService
    {
        private readonly ILevelProgressRepository _repository;

        public LevelProgressService(ILevelProgressRepository repository)
        {
            _repository = repository;
        }

        public void CompleteLevel(string levelId, int score, string nextLevelId)
        {
            _repository.SaveProgress(new LevelProgressData(levelId, score));
            _repository.UnlockLevel(nextLevelId);
        }

        public int GetScore(string levelId) => _repository.GetProgress(levelId).Score;

        public bool IsUnlocked(string levelId) => _repository.IsLevelUnlocked(levelId);
        public void ResetProgress() => _repository.ResetProgress();

        public int GetLastUnlockedLevelId()
        {
            int i = 0;
            while (_repository.IsLevelUnlocked(i.ToString())) i++;

            return i - 1;
        }
    }
}
