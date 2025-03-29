using System.Collections.Generic;
using Application.Dto;
using Domain.Entities;

namespace Presentation.Services
{
    public class GameService
    {
        private List<Card> _cards = new();
        public readonly TotalCardStatsDto TotalCardStats = new();
        private Boss _currentBoss;
        private int _currentZone = 1;

        public int CurrentZone => _currentZone;
        public Boss CurrentBoss => _currentBoss;
        public void Initialize(List<Card> cards, Boss initialBoss)
        {
            _cards = cards;
            TotalCardStats.Cards = cards;
            _currentBoss = initialBoss;
            _currentZone = 1;
        }

        public bool AreAllCardsDead()
        {
            return TotalCardStats.Hp <= 0;
        }

        public bool IsBossDead()
        {
            return _currentBoss.CurrentHp <= 0;
        }

        public (float effectiveDamage, bool bossDefeated) AttackBoss()
        {
            float totalAttack = TotalCardStats.Attack;
            float effectiveDamage = _currentBoss.TakeDamage(totalAttack);
            bool bossDefeated = IsBossDead();

            return (effectiveDamage, bossDefeated);
        }

        public void BossAttack()
        {
            foreach (var card in _cards)
            {
                card.TakeDamage(_currentBoss.Attack);
            }
        }

        public void AdvanceToNextZone()
        {
            _currentZone++;

            // Апгрейд босса
            _currentBoss.UpgradeForNextZone(_currentZone);

            // Апгрейд карт
            foreach (var card in _cards)
            {
                card.ApplyLevelUpBonus();
            }
        }
    }
}