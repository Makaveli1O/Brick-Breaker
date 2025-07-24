
namespace Assets.Scripts.Level.Config
{
    public record LevelConfig
    {
        public BallConfig BallConfig;

        public LevelConfig(
            BallConfig ballConfig
        )
        {
            BallConfig = ballConfig;
        }
    }
}