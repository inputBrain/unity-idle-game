namespace Domain.Entities
{
    public class Zone
    {
        public ReactiveProperty<int> CurrentZone { get; } = new();
    }
}