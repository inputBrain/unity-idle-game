using System;
using System.Collections.Generic;

namespace Domain
{
    public class ReactiveProperty<T>
    {
        private T _value;
        public event Action<T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        // Оператор неявного преобразования позволяет получать значение напрямую
        public static implicit operator T(ReactiveProperty<T> reactive)
        {
            return reactive.Value;
        }
    }
}