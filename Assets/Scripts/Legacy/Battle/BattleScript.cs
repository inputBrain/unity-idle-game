using System;
using System.Collections.Generic;
using Application.Dto;
using Domain.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OLd.Battle
{
    public class BattleScript : MonoBehaviour
    {
        public List<Card> CardList = new();
        public int Zone = 1;

        public TotalCardStatsDto TotalCardStat = new();
        public Boss Boss = new();

        public Boss Enemy = new();
        
        private float enemyCurrentHP;
        


        private void Start()
        {
            enemyCurrentHP = Enemy.Hp;

           InvokeRepeating(nameof(BattleUpdate), 0f, 1f);
        }


        private void FixedUpdate()
        {
            foreach (var card in CardList)
            {
                card.CurrentHp += TotalCardStat.HpRegeneration;
                if (card.CurrentHp > card.MaxHp)
                {
                    card.CurrentHp = card.MaxHp;
                }
            }
        }


        private void BattleUpdate()
        {
            DealDamageToBoss();
            DealDamageToTeam();

            if (enemyCurrentHP <= 0)
            {
                OnBossDefeated();
            }

            if (TotalCardStat.Hp <= 0)
            {
                OnEnemyWin();
            }
        }
        
        
        private void DealDamageToBoss()
        {
            var totalDamage = TotalCardStat.Attack;

            // Critical attack case
            if (Random.value * 100 < TotalCardStat.Crit)
            {
                totalDamage *= 1 + TotalCardStat.CritDmg / 10f;
                enemyCurrentHP -= totalDamage;
                if (enemyCurrentHP <= 0)
                {
                    enemyCurrentHP = 0;
                }
                return;
            }

            
            enemyCurrentHP -= totalDamage;
            if (enemyCurrentHP <= 0)
            {
                enemyCurrentHP = 0;
            }
        }

        
        private void DealDamageToTeam()
        {
            var damageToTeam = Zone % 10 == 0 ? Boss.Attack : Enemy.Attack;
            
            if (Random.value * 100 < TotalCardStat.Evade)
            {
                return;
            }
            
            if (Random.value * 100 < TotalCardStat.Block)
            {
                damageToTeam *= (1 - TotalCardStat.BlockPower / 100f);

                TotalCardStat.GetDamage(damageToTeam);
            }

            TotalCardStat.GetDamage(damageToTeam);
        }
        

        private void OnBossDefeated()
        {
            ReceiveExp();
            
            if (Random.value * 100 < 50f)
            {
                Debug.Log("Reward granted!");
            }
            
            Zone++;
        }
        
        
        private void OnEnemyWin()
        {
            Zone--;
            if (Zone <= 0)
            {
                Zone = 1;
            }
        }

        
        private void ReceiveExp()
        {
            if (CardList.Count <= 0)
            {
                return;
            }

            float experience = Zone % 10 == 0 ? Boss.ExpReward : Enemy.ExpReward;
            
            var expPerCard = experience / CardList.Count;

            foreach (var card in CardList)
            {
                card.ExpCurrent += expPerCard;
                if (card.ExpCurrent >= card.ExpToNextLevel)
                {
                    while (card.ExpCurrent >= card.ExpToNextLevel)
                    {
                        card.Level += 1;
                        card.ExpCurrent -= card.ExpToNextLevel;
                        card.ExpToNextLevel = CalculateExpToNextLevel(card);
                    }
                }
            }
        }

        
        private static float CalculateExpToNextLevel(Card card)
        {
            var scale = card.Rarity switch
            {
                (int)Rarity.Common => 1.1f,
                (int)Rarity.Rare => 1.2f,
                (int)Rarity.Epic => 1.5f,
                (int)Rarity.Legendary => 2.5f,
                _ => throw new ArgumentException($"Undefined card rarity: {card.Rarity}. Cannot calculate experience")
            };

            return card.StartBaseExp * Mathf.Pow(card.Level, scale);
        }
    }
}
