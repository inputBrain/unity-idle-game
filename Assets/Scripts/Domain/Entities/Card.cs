using System;
using UnityEngine;

namespace Domain.Entities
{
  
    public class Card : BaseEntity
    {
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
        
        public float StartBaseExp { get; set; }
    }
}