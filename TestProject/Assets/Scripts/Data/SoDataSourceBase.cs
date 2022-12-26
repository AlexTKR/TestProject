using UnityEngine;

namespace Data
{
    public abstract class SoDataSourceBase<T> : ScriptableObject where T : class
    {
        public abstract void Compose(T instance);
    }
}