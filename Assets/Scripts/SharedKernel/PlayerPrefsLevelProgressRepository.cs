using UnityEngine;

namespace Assets.Scripts.SharedKernel
{
    public class PlayerPrefsLevelProgressRepository : ILevelProgressRepository
    {
        private const string LevelPrefix = "level_";
        private const string UnlockedPrefix = "unlocked_";

        public LevelProgressData GetProgress(string levelId)
        {
            string key = LevelPrefix + levelId;
            int score = PlayerPrefs.GetInt(key, 0);
            return new LevelProgressData(levelId, score);
        }

        public void SaveProgress(LevelProgressData data)
        {
            string key = LevelPrefix + data.LevelId;
            int current = PlayerPrefs.GetInt(key, 0);

            if (data.Score > current)
                PlayerPrefs.SetInt(key, data.Score);
        }

        public bool IsLevelUnlocked(string levelId)
        {
            return PlayerPrefs.GetInt(UnlockedPrefix + levelId, 0) == 1;
        }

        public void UnlockLevel(string levelId)
        {
            PlayerPrefs.SetInt(UnlockedPrefix + levelId, 1);
        }
    }
}
