
namespace Assets.Scripts.Level.Config
{
    public record LevelConfig
    {
        public BallConfig BallConfig;
        public int InitialHP;

        public LevelConfig(
            BallConfig ballConfig,
            int initialHP = 5
        )
        {
            BallConfig = ballConfig;
            InitialHP = initialHP;
        }
    }
}