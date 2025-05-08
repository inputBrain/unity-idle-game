namespace Model.Boss
{
    public class BossModel : ReactiveProperty<BossModel>
    {
        public float BaseHp { get; set; }

        public ReactiveProperty<float> CurrentHp { get; } = new();
        public ReactiveProperty<float> MaxHp { get; } = new();
        public ReactiveProperty<float> Attack { get; } = new();
        public ReactiveProperty<int> ExpReward { get; } = new();
        public ReactiveProperty<int> GoldReward { get; } = new();

        public void GetUpdatedStats(int zone)
        {
            MaxHp.Value = BaseHp * zone;
            CurrentHp.Value = MaxHp.Value;
            Attack.Value += 100f;
        }
    }
}