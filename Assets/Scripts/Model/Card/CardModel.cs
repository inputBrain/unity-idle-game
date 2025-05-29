using System;
using Model.InventoryCard;

namespace Model.Card
{
    public class CardModel : IInventoryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ReactiveProperty<string> IconResourcesPath { get; } = new();
        public Guid InstanceId { get; set; }
        public float StartBaseExp { get; set; }
        
        public ReactiveProperty<int> Rank { get; } = new();

        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<float> ExpCurrent { get; } = new();
        public ReactiveProperty<float> ExpToNextLevel { get; } = new();
        public ReactiveProperty<float> CurrentHp { get; } = new();
        public ReactiveProperty<float> MaxHp { get; } = new();
        public ReactiveProperty<float> HpRegeneration { get; } = new();
        public ReactiveProperty<float> Attack { get; } = new();
        public ReactiveProperty<float> Crit { get; } = new();
        public ReactiveProperty<float> CritDmg { get; } = new();
        public ReactiveProperty<float> Block { get; } = new();
        public ReactiveProperty<float> BlockPower { get; } = new();
        public ReactiveProperty<float> Evade { get; } = new();
        public ReactiveProperty<int> Rarity { get; } = new();
        
        public ReactiveProperty<int> Count { get; } = new();
        public ReactiveProperty<float> DropChance { get; } = new();
    }
}