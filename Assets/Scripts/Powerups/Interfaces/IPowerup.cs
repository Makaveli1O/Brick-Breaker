namespace Assets.Scripts.Powerups
{
    public interface IPowerup
    {
        void Apply();
        float GetDuration();
    }
}