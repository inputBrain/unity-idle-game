using System;
using System.Collections.Generic;
using Api.Payload;
using Model.Boss;
using Model.Card;
using Model.Zone;
using Presentation.Inventory;
using Presentation.TotalStats;
using Services;
using UnityEngine;

namespace Battle
{
    public class BattleScript
    {
        public List<CardModel> CardList = new();

        public TotalToolbarStatsViewModel TotalToolbarStatistic = new();
        public BossModel BossModel = new();
        private ZoneModel _zoneModel;
        private readonly InventoryPresenter _inventoryPresenter;

        public BattleScript(BossModel bossModel, ZoneModel zoneModel, List<CardModel> cards, InventoryPresenter inventoryPresenter)
        {
            CardList = cards;
            _inventoryPresenter = inventoryPresenter;
            BossModel = bossModel;
            _zoneModel = zoneModel;
            TotalToolbarStatistic.Cards = cards;

            _zoneModel.CurrentZone.Value = 1;
        }
        
        public void BattleUpdate()
        {
            // HpRegeneration();
            DealDamageToBoss();
            DealDamageToTeam();

            if (BossModel.CurrentHp <= 0)
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

            
            BossModel.CurrentHp.Value -= totalDamage;
        }

        
        private void DealDamageToTeam()
        {
            var damageToTeam = BossModel.Attack;
            
            
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
            
            var allCards = new CardLoaderService().GetDomainCards();
            var dropService = new CardDropService(allCards);
            // var droppedCards = dropService.RollMultipleDrops(1);
            var droppedCard = dropService.RollDrop();
            if (droppedCard != null)
            {
                Debug.Log($"Dropped card: {droppedCard.Title}");
                _inventoryPresenter.AddOrStackCard(droppedCard);
            }

            // foreach (var card in droppedCards)
            // {
            //     Debug.Log($"Dropped card: {card.Title} (Rarity: {card.Rarity.Value})");
            //     // Добавляем в инвентарь:
            //     // Вызов InventoryPresenter.AddItemToInventory(card); если у тебя есть к нему доступ
            //
            //     _inventoryPresenter.AddOrStackCard(card);
            // }
            _zoneModel.CurrentZone.Value++;
            
            BossModel.GetUpdatedStats(_zoneModel.CurrentZone);
            
        }
        
        
        private void OnEnemyWin()
        {
            _zoneModel.CurrentZone.Value--;
            if (_zoneModel.CurrentZone <= 0)
            {
                _zoneModel.CurrentZone.Value = 1;
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

        
        private static float CalculateExpToNextLevel(CardModel cardModel)
        {
            var scale = cardModel.Rarity.Value switch
            {
                (int)Rarity.Common => 1.01f,
                (int)Rarity.Rare => 1.02f,
                (int)Rarity.Epic => 1.05f,
                (int)Rarity.Legendary => 1.1f,
                _ => throw new ArgumentException($"Undefined card rarity: {cardModel.Rarity}. Cannot calculate experience")
            };

            return cardModel.StartBaseExp * Mathf.Pow(cardModel.Level, scale);
        }
    }
}
