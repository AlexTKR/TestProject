using System;
using System.Collections.Generic;
using UnityEngine;

namespace Context
{
    public class ProjectContext : Singleton<ProjectContext>
    {
        private Dictionary<Type, object> _singleContainer
            = new Dictionary<Type, object>();

        public void RegisterAsSingle(Type type, object value)
        {
            _singleContainer[type] = value;
        }
        
        public void RegisterAsSingle<T>(object value)
        {
            _singleContainer[typeof(T)] = value;
        }
        
        public void RegisterAsSingle(Type[] types, object value)
        {
            for (int i = 0; i < types.Length; i++)
            {
                RegisterAsSingle(types[i], value);
            }
        }

        public T Get<T>()
        {
            if (_singleContainer.TryGetValue(typeof(T), out var value))
                return (T)value;

            throw new Exception("Instance of type is not registered");
        }
    }
}