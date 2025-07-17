namespace Assets.Scripts.Level
{
    public class LevelProgressData
    {
        public string LevelId { get; }
        public int Score { get; }

        public LevelProgressData(string levelId, int score)
        {
            LevelId = levelId;
            Score = score;
        }
    }
}
