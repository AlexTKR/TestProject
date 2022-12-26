using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Context
{
    public class SceneContext : Singleton<SceneContext>
    {
        private Dictionary<int, Dictionary<Type, object>> _container
            = new Dictionary<int, Dictionary<Type, object>>();

        public void Register<T>(object value, int? sceneIndex = null)
        {
              Register(value, typeof(T), sceneIndex);
        }

        public void Register(Type[] types, object value, int? sceneIndex = null)
        {
            for (int i = 0; i < types.Length; i++)
            {
                Register(value, types[i], sceneIndex);
            }
        }

        private void Register(object value, Type type , int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            if (!_container.ContainsKey(sceneIndex.Value))
                _container[sceneIndex.Value] = new Dictionary<Type, object>();

            _container[sceneIndex.Value][type] = value;
        }

        public T Get<T>(int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            if (!_container.ContainsKey(sceneIndex.Value))
                throw new Exception("Scene is not registered");

            if (_container[sceneIndex.Value].TryGetValue(typeof(T), out var value))
                return (T)value;

            throw new Exception("Instance of type is not registered");
        }

        public void ClearSceneContext(int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            _container[sceneIndex.Value] = new Dictionary<Type, object>();
        }
    }
}