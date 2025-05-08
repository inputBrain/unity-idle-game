namespace Model.Zone
{
    public class ZoneModel
    {
        public ReactiveProperty<int> CurrentZone { get; } = new();
    }
}