namespace DataSource
{
    public interface IDataSource<T>
    {
        T Load();
    }
}
