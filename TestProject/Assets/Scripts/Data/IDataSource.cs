namespace Data
{
    public interface IDataSource<T>
    {
        T Load();
    }
}
