using System;
using System.Collections.Generic;
using UnityEngine;

namespace Context
{
    public class ProjectContext : Singleton<ProjectContext>
    {
        private Dictionary<Type, object> _container
            = new Dictionary<Type, object>();

        public void Register(Type type, object value)
        {
            _container[type] = value;
        }
        
        public void Register<T>(object value)
        {
            _container[typeof(T)] = value;
        }
        
        public void Register(Type[] types, object value)
        {
            for (int i = 0; i < types.Length; i++)
            {
                Register(types[i], value);
            }
        }

        public T Get<T>()
        {
            if (_container.TryGetValue(typeof(T), out var value))
                return (T)value;

            throw new Exception("Instance of type is not registered");
        }
    }
}