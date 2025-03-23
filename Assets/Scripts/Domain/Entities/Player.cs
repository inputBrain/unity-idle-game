namespace Domain.Entities
{
    public class Player
    {
        private int _coins;

        public int Coins
        {
            get => _coins;
            set
            {
                if (_coins == value) return;
                _coins = value;
                OnCoinsChanged?.Invoke(_coins);
            }
        }

        public event System.Action<int> OnCoinsChanged;
    }
}