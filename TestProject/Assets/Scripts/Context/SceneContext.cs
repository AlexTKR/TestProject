using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Context
{
    public class SceneContext : Singleton<SceneContext>
    {
        private Dictionary<int, Dictionary<Type, object>> _singleContainer
            = new Dictionary<int, Dictionary<Type, object>>();

        private Dictionary<int, Dictionary<Type, List<object>>> _multipleContainer =
            new Dictionary<int, Dictionary<Type, List<object>>>();

        public void RegisterAsSingle<T>(object value, int? sceneIndex = null)
        {
            RegisterAsSingle(value, typeof(T), sceneIndex);
        }

        public void RegisterAsSingle(Type[] types, object value, int? sceneIndex = null)
        {
            for (int i = 0; i < types.Length; i++)
            {
                RegisterAsSingle(value, types[i], sceneIndex);
            }
        }

        private void RegisterAsSingle(object value, Type type, int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            if (!_singleContainer.ContainsKey(sceneIndex.Value))
                _singleContainer[sceneIndex.Value] = new Dictionary<Type, object>();

            _singleContainer[sceneIndex.Value][type] = value;
        }

        public void RegisterAsMultiple<T>(object[] instances, int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;
            var type = typeof(T);

            if (!_multipleContainer.ContainsKey(sceneIndex.Value))
                _multipleContainer[sceneIndex.Value] = new Dictionary<Type, List<object>>();

            if (!_multipleContainer[sceneIndex.Value].ContainsKey(type))
            {
                _multipleContainer[sceneIndex.Value][type] = new List<object>();
            }

            for (int i = 0; i < instances.Length; i++)
            {
                var instance = instances[i];
                _multipleContainer[sceneIndex.Value][type].Add(instance);
            }
        }


        public T GetSingle<T>(int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            if (!_singleContainer.ContainsKey(sceneIndex.Value))
                throw new Exception("Scene is not registered");

            if (_singleContainer[sceneIndex.Value].TryGetValue(typeof(T), out var value))
                return (T)value;

            throw new Exception("Instance of type is not registered");
        }

        public IList<T> GetMultiple<T>(int? sceneIndex = null) where T : class
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            if (!_multipleContainer.ContainsKey(sceneIndex.Value))
                throw new Exception("Scene is not registered");

            if (_multipleContainer[sceneIndex.Value].TryGetValue(typeof(T), out var value))
                return value.Select(o => o as T).ToArray();

            throw new Exception("Instance of type is not registered");
        }

        public void ClearSceneContext(int? sceneIndex = null)
        {
            sceneIndex ??= SceneManager.GetActiveScene().buildIndex;

            _singleContainer[sceneIndex.Value] = new Dictionary<Type, object>();
        }
    }
}