namespace Domain.Entities
{
    public class Zone
    {
        private int _currentZone;
        
        public int CurrentZone
        {
            get => _currentZone;
            set
            {
                if (_currentZone == value)
                {
                    return;
                }
                {
                    _currentZone = value;
                    OnCurrenZoneChanged?.Invoke(_currentZone);
                }
            }
        }

        public event System.Action<int> OnCurrenZoneChanged;
    }
}