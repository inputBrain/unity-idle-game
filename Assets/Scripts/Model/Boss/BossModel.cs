namespace Model.Boss
{
    public class BossModel : ReactiveProperty<BossModel>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float BaseHp { get; set; }
        public float BaseAttack { get; set; }
        public float BaseExperienceReward { get; set; }
        public float BaseGoldReward { get; set; }

        public ReactiveProperty<string> IconResourcesPath { get; } = new();
        
        public ReactiveProperty<float> CurrentHp { get; } = new();
        public ReactiveProperty<float> MaxHp { get; } = new();
        public ReactiveProperty<float> Attack { get; } = new();
        public ReactiveProperty<float> ExpReward { get; } = new();
        public ReactiveProperty<float> GoldReward { get; } = new();

        public void GetUpdatedStats(int zone)
        {
            MaxHp.Value = BaseHp * zone;
            CurrentHp.Value = MaxHp.Value;
            Attack.Value = BaseAttack * (zone / 10f);
            
            ExpReward.Value =  BaseExperienceReward * (zone / 10f);
            GoldReward.Value =  BaseGoldReward * (zone / 10f);
        }
    }
}