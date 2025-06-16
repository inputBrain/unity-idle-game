using System.Collections.Generic;
using System.Linq;
using Model.Card;
using UnityEngine;

namespace Presentation.TotalStats
{
    /// <summary>
    /// Main stats model
    /// </summary>
    public class TotalToolbarStatsViewModel
    {
        public IList<CardModel> Cards = new List<CardModel>();
        public float Hp => Cards.Sum(card => card.CurrentHp);
        public float HpRegeneration => Cards.Sum(card => card.HpRegeneration);
        public float Attack => Cards.Sum(card => card.Attack);
        public float Crit => Cards.Sum(card => card.Crit);
        public float CritDmg => Cards.Sum(card => card.CritDmg);
        public float Block => Cards.Sum(card => card.Block);
        public float BlockPower => Cards.Sum(card => card.BlockPower);
        public float Evade => Cards.Sum(card => card.Evade);


        public void GetDamage(float damage)
        {
            if (Cards == null || Cards.Count == 0)
                return;

            if (Cards.Sum(c => c.CurrentHp) - damage < 0)
            {
                foreach (var c in Cards)
                {
                    c.CurrentHp.Value = 0;
                }
                return;
            }
                
            foreach (var card in Cards)
            {
                card.CurrentHp.Value -= damage / Cards.Count;
            }
        }

        public void RegenerateHp(float deltaTime)
        {
            foreach (var card in Cards)
            {
                var target = card.MaxHp.Value;
                var step   = card.HpRegeneration.Value * deltaTime;
                card.CurrentHp.Value = Mathf.MoveTowards(card.CurrentHp.Value, target, step);
            }
        }
    }
}
