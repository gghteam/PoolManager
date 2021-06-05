namespace PoolManager
{
    public interface IFactory<T>
    {
        T Create();
    }
}

