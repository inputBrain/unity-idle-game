namespace Domain.Entities 
{
    public class Boss
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Armor { get; set; }
        public float Hp { get; set; }
        public float MaxHp { get; set; }
        public float Attack { get; set; }
        public float StartAttack { get; set; }
        public int ExpReward { get; set; }
        public int GoldReward { get; set; }
        public string ImageResourcePath { get; set; }
        
        public float TakeDamage(float incomingDamage)
        {
            float effectiveDamage = incomingDamage - Armor;
            if (effectiveDamage < 0)
                effectiveDamage = 0;

            Hp -= effectiveDamage;
            if (Hp < 0)
                Hp = 0;
                
            return effectiveDamage;
        }
        
        public void UpgradeForNextZone(int zoneNumber)
        {
            MaxHp += 20 * zoneNumber;
            Hp = MaxHp;
            Attack = StartAttack + 5 * zoneNumber;
        }
    }
}