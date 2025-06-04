using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly TotalToolbarStatsViewModel _stats;
        private readonly InventoryPresenter       _inventoryPresenter;
        private readonly CardDropService          _dropService;
        private readonly BossModel                _bossModel;
        private readonly ZoneModel                _zoneModel;

        public BattleScript(
            BossModel bossModel,
            ZoneModel zoneModel,
            List<CardModel> allDomainCards,
            InventoryPresenter inventoryPresenter,
            TotalToolbarStatsViewModel statsModel)
        {
            _bossModel          = bossModel;
            _zoneModel          = zoneModel;
            _inventoryPresenter = inventoryPresenter;
            _stats              = statsModel;
            _dropService        = new CardDropService(allDomainCards);
        }
        
        public void BattleUpdate()
        {
            HpRegeneration();
            DealDamageToBoss();
            DealDamageToTeam();

            if (_bossModel.CurrentHp.Value <= 0)
                OnBossDefeated();

            if (_stats.Hp <= 0)
                OnEnemyWin();
        }


        private void HpRegeneration()
        {
            if (_stats.Cards == null || _stats.Cards.Count == 0)
                return;

            foreach (var card in _stats.Cards)
            {
                card.CurrentHp.Value = Mathf.Min(
                    card.CurrentHp.Value + _stats.HpRegeneration,
                    card.MaxHp.Value);
            }
        }
     
        
        private void DealDamageToBoss()
        {
            if (_stats.Cards == null || _stats.Cards.Count == 0)
                return;

            var totalDamage = _stats.Cards.Sum(card => card.Attack.Value);

            // Critical attack case TODO:
            // if (Random.value * 100 < _stats.Crit)
            // {
            //     totalDamage *= 1 + _stats.CritDmg / 10f;
            // }

            _bossModel.CurrentHp.Value -= totalDamage;
        }

        
        private void DealDamageToTeam()
        {
            var dmgToTeam = _bossModel.Attack.Value;
            _stats.GetDamage(dmgToTeam);
            
                //TODO:
            // if (Random.value * 100 < _stats.Evade)
            // {
            //     return;
            // }
            //
            // if (Random.value * 100 < _stats.Block)
            // {
            //     damageToTeam *= (1 - _stats.BlockPower / 100f);
            // }
        }
        

        private void OnBossDefeated()
        {
            ReceiveExp();

            var drop = _dropService.RollDrop();
            if (drop != null)
            {
                Debug.Log($"Dropped card: {drop.Title}");
                _inventoryPresenter.AddOrStackCard(drop);
                
                
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
            
            _bossModel.GetUpdatedStats(_zoneModel.CurrentZone);
            
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
            var toolbarCards = _stats.Cards;
            if (toolbarCards == null || toolbarCards.Count == 0)
                return;

            const float totalExperience = 200f;
            var expPerCard = totalExperience / toolbarCards.Count;

            foreach (var card in toolbarCards)
            {
                card.ExpCurrent.Value += expPerCard;

                while (card.ExpCurrent.Value >= card.ExpToNextLevel.Value && card.Level.Value < CardModel.MaxLevel)
                {
                    card.ExpCurrent.Value -= card.ExpToNextLevel.Value;

                    card.Level.Value += 1;
                    if (card.Rank.Value < CardModel.MaxRank)
                        card.Rank.Value += 1;

                    card.MaxHp.Value      += 1.1f;
                    card.CurrentHp.Value  += 1.1f;
                    card.Attack.Value     += 1.1f;
                    card.Evade.Value      += 1.1f;
                    card.Block.Value      += 1.1f;
                    card.BlockPower.Value += 1.1f;

                    card.ExpToNextLevel.Value = CalculateExpToNextLevel(card);

                    if (card.Level.Value >= CardModel.MaxLevel)
                    {
                        card.Level.Value = CardModel.MaxLevel;
                        card.ExpCurrent.Value = card.ExpToNextLevel.Value;
                        break;
                    }
                }

                if (card.Level.Value >= CardModel.MaxLevel)
                {
                    card.ExpCurrent.Value = card.ExpToNextLevel.Value;
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
