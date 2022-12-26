using System;
using System.Collections.Generic;

namespace ViewModel
{
    public interface IGetViewModel
    {
        T Get<T>() where T : class;
    }

    public class ViewModelProvider : IGetViewModel
    {
        private Dictionary<Type, object> _container
            = new Dictionary<Type, object>();

        public ViewModelProvider()
        {
            _container[typeof(ViewModelBase<IMainPanel>)] = new MainPanelViewModel();
        }

        public T Get<T>() where T : class
        {
            if (!_container.TryGetValue(typeof(T), out var value))
                return null;

            return (T)value;
        }
    }
}