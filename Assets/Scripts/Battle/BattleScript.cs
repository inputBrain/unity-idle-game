using System;
using System.Collections.Generic;
using Application.Dto;
using Domain.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleScript
    {
        public List<Card> CardList = new();

        public TotalToolbarStatisticDto TotalToolbarStatistic = new();
        public Boss Boss = new();
        private Zone _zone;
        


        public BattleScript(Boss boss, Zone zone, List<Card> cards)
        {
            CardList = cards;
            Boss = boss;
            _zone = zone;
            TotalToolbarStatistic.Cards = cards;

            _zone.CurrentZone.Value = 1;
        }
        
        public void BattleUpdate()
        {
            // HpRegeneration();
            DealDamageToBoss();
            DealDamageToTeam();

            if (Boss.CurrentHp <= 0)
            {
                OnBossDefeated();
            }

            if (TotalToolbarStatistic.Hp <= 0)
            {
                OnEnemyWin();
            }
        }


        private void HpRegeneration()
        {
            foreach (var card in CardList)
            {
                card.CurrentHp.Value += TotalToolbarStatistic.HpRegeneration;
                if (card.CurrentHp > card.MaxHp)
                {
                    card.CurrentHp.Value = card.MaxHp;
                }
            }
        }
     
        
        private void DealDamageToBoss()
        {
            var totalDamage = TotalToolbarStatistic.Attack;

            // Critical attack case TODO:
            // if (Random.value * 100 < TotalToolbarStatistic.Crit)
            // {
            //     totalDamage *= 1 + TotalToolbarStatistic.CritDmg / 10f;
            // }

            
            Boss.CurrentHp.Value -= totalDamage;
        }

        
        private void DealDamageToTeam()
        {
            var damageToTeam = Boss.Attack;
            
            
                //TODO:
            // if (Random.value * 100 < TotalToolbarStatistic.Evade)
            // {
            //     return;
            // }
            //
            // if (Random.value * 100 < TotalToolbarStatistic.Block)
            // {
            //     damageToTeam *= (1 - TotalToolbarStatistic.BlockPower / 100f);
            // }

            TotalToolbarStatistic.GetDamage(damageToTeam);
        }
        

        private void OnBossDefeated()
        {
            ReceiveExp();
            
            if (Random.value * 100 < 50f)
            {
                Debug.Log("Reward granted!");
            }
            
            _zone.CurrentZone.Value++;
            
            Boss.GetUpdatedStats(_zone.CurrentZone);
            
        }
        
        
        private void OnEnemyWin()
        {
            _zone.CurrentZone.Value--;
            if (_zone.CurrentZone <= 0)
            {
                _zone.CurrentZone.Value = 1;
            }
            
            // Boss.Attack -= 100f;

        }

        
        private void ReceiveExp()
        {
            if (CardList.Count <= 0)
            {
                return;
            }

            //TODO: implement reward for boss wave
            float experience = 200f;
            
            var expPerCard = experience / CardList.Count;

            foreach (var card in CardList)
            {
                card.ExpCurrent.Value += expPerCard;
                if (card.ExpCurrent >= card.ExpToNextLevel)
                {
                    while (card.ExpCurrent >= card.ExpToNextLevel)
                    {
                        card.Level.Value += 1;
                        card.MaxHp.Value += 1.1f;
                        card.CurrentHp.Value += 1.1f;
                        //TODO:
                        // card.HpRegeneration *= 1.1f;
                        card.Attack.Value += 1.1f;
                        card.Evade.Value += 1.1f;
                        card.Block.Value += 1.1f;
                        card.BlockPower.Value += 1.1f;
                        card.ExpCurrent.Value -= card.ExpToNextLevel;
                        card.ExpToNextLevel.Value = CalculateExpToNextLevel(card);
                    }
                }
            }
        }

        
        private static float CalculateExpToNextLevel(Card card)
        {
            var scale = card.Rarity.Value switch
            {
                (int)Rarity.Common => 1.01f,
                (int)Rarity.Rare => 1.02f,
                (int)Rarity.Epic => 1.05f,
                (int)Rarity.Legendary => 1.1f,
                _ => throw new ArgumentException($"Undefined card rarity: {card.Rarity}. Cannot calculate experience")
            };

            return card.StartBaseExp * Mathf.Pow(card.Level, scale);
        }
    }
}
