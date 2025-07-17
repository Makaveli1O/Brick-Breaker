namespace Assets.Scripts.SharedKernel
{
    public interface IObjectPool<T>
    {
        T Get();
        void Return(T item);
    }
}
