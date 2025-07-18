namespace Assets.Scripts.SharedKernel
{
    public interface IConfigurableBehaviour<TConfig>
    {
        void Configure(TConfig config);
    }
}