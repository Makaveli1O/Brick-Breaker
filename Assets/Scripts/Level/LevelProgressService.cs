namespace Assets.Scripts.Level
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
    }
}
