using Assets.Scripts.Level.Config;
using UnityEngine;

public static class LevelConfigFactory
{
    public static LevelConfig WithBall(Vector2? launchDirection = null, Vector2? position = null, float? initialPush = null)
    {
        return new LevelConfig(new BallConfig
        {
            LaunchDirection = launchDirection,
            BallInitialPosition = position,
            BallInitialPush = initialPush
        });
    }  
    public static LevelConfig WithHP(int hp)
    {
        return new LevelConfig(new BallConfig(), hp);
    }
}
