using System;

namespace ECS.Pools
{
    public interface IEntityPool<T>
    {
        T Get();
        void Return(T instance);
    }

    public class SimpleEntityPool<T> : IEntityPool<T> where T : class
    {
        private T[] _pooled;
        private int _entityCount;

        public SimpleEntityPool(int defaultCount = 20)
        {
            _pooled = new T[defaultCount];
        }

        public T Get()
        {
            if (_entityCount <= 0)
                return null;

            _entityCount--;
            var entity = _pooled[_entityCount];
            _pooled[_entityCount] = null;
            return entity;
        }

        public void Return(T entity)
        {
            if (_entityCount >= _pooled.Length)
                Array.Resize(ref _pooled, (int)(_pooled.Length + _pooled.Length * 0.5));

            _pooled[_entityCount] = entity;
            _entityCount++;
        }
    }
}