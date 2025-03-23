namespace Domain.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public float ExpCurrent { get; set; }
        public float ExpToNextLevel { get; set; }
        public float StartBaseExp { get; set; }
        public float CurrentHp { get; set; }
        public float MaxHp { get; set; }
        public float HpRegeneration { get; set; }
        public float Attack { get; set; }
        public float Crit { get; set; }
        public float CritDmg { get; set; }
        public float Block { get; set; }
        public float BlockPower { get; set; }
        public float Evade { get; set; }
        public int Rarity { get; set; }
        public string ImageResourcePath { get; set; }
        
        public void TakeDamage(float damage)
        {
            CurrentHp -= damage;
            if (CurrentHp < 0)
                CurrentHp = 0;
        }
        
        public void ApplyLevelUpBonus()
        {
            MaxHp *= 1.2f;
            Attack *= 1.1f;
            CurrentHp = MaxHp;
        }
    }
}