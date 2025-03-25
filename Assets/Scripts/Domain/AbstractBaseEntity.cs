using System.Collections.Generic;

namespace Domain
{
    public abstract class AbstractBaseEntity
    {
        private readonly Dictionary<string, object> _properties = new();


        protected ObservableProperty<T> RegisterProperty<T>(string propertyName, T initialValue)
        {
            var property = new ObservableProperty<T>(initialValue);
            _properties[propertyName] = property;
            return property;
        }


        public ObservableProperty<T> GetProperty<T>(string propertyName)
        {
            return _properties[propertyName] as ObservableProperty<T>;
        }
    }
}