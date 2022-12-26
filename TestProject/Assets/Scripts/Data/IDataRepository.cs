namespace Data
{
    public interface IDataRepository<T>
    {
        T Data { get; }
    }
}