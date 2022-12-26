using System;

namespace Data
{
    public class SoDataSourceProvider<TData> : IDataSource<TData> where TData : class
    {
        private SoDataSourceBase<TData> _source;
        
        public SoDataSourceProvider(SoDataSourceBase<TData> source)
        {
            _source = source;
        }

        public TData Load()
        {
            var data = Activator.CreateInstance<TData>();
            _source.Compose(data);
            return data;
        }
    }
}
