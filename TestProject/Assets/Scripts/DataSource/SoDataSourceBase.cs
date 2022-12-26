using UnityEngine;

namespace DataSource
{
    public abstract class SoDataSourceBase<T> : ScriptableObject where T : class
    {
        public abstract void Compose(T instance);
    }
}