using System;

namespace Domain
{
    public class ObservableProperty<T>
    {
        private T _value;
        private readonly Func<T, T, bool> _equalityComparer;

        public event Action<T> OnValueChanged;

        public ObservableProperty(T initialValue, Func<T, T, bool> equalityComparer = null)
        {
            _value = initialValue;
            _equalityComparer = equalityComparer ?? ((a, b) => Equals(a, b));
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_equalityComparer(_value, value))
                    return;

                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public static implicit operator T(ObservableProperty<T> property) => property._value;
    }
}