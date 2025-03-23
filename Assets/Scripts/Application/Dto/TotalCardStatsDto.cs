using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Application.Dto
{
    public class TotalCardStatsDto
    {
        public IList<Card> Cards = new List<Card>();

        public float ExpCurrent { get; set; }
        public float ExpToNextLevel { get; set; }
        public float Hp => Cards.Sum(card => card.CurrentHp);
        public float HpRegeneration => Cards.Sum(card => card.HpRegeneration);
        public float Attack => Cards.Sum(card => card.Attack);
        public float Crit => Cards.Sum(card => card.Crit);
        public float CritDmg => Cards.Sum(card => card.CritDmg);
        public float Block => Cards.Sum(card => card.Block);
        public float BlockPower => Cards.Sum(card => card.BlockPower);
        public float Evade => Cards.Sum(card => card.Evade);
    }
}