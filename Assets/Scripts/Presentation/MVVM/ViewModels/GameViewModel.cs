using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Presentation.Services;

namespace Presentation.MVVM.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private readonly CardLoaderService _cardLoaderService;
        private readonly GameService _gameService;
        
        private List<CardViewModel> _cardViewModels = new();
        private BossViewModel _bossViewModel;
        
        private int _currentZone;
        public int CurrentZone
        {
            get => _currentZone;
            private set => this.SetProperty(ref _currentZone, value);
        }

        private string _battleMessage = string.Empty;
        public string BattleMessage
        {
            get => _battleMessage;
            set => this.SetProperty(ref _battleMessage, value);
        }
        
        private float _totalCardHp;
        public float TotalCardHp
        {
            get => _totalCardHp;
            private set => this.SetProperty(ref _totalCardHp, value);
        }
        
        private float _totalCardAttack;
        public float TotalCardAttack
        {
            get => _totalCardAttack;
            private set => this.SetProperty(ref _totalCardAttack, value);
        }

        public event Action<float> CardsTotalHpChanged;
        public event Action<float> CardsTotalDamageChanged;
        public event Action<int> ZoneChanged;
        public event Action<string> BattleMessageSent;
        public event Action GameOver;

        public IReadOnlyList<CardViewModel> CardViewModels => _cardViewModels;
        public BossViewModel CurrentBoss => _bossViewModel;

        public GameViewModel(CardLoaderService cardLoaderService)
        {
            _cardLoaderService = cardLoaderService;
            _gameService = new GameService();
        }

        public void InitializeGame(int maxCardCount)
        {
            var domainCards = _cardLoaderService.GetDomainCards();
            if (domainCards == null || domainCards.Count == 0)
            {
                SendBattleMessage("Ошибка: Нет загруженных карточек!");
                return;
            }

            var selectedCards = domainCards.Take(maxCardCount).ToList();
            
            var initialBoss = new Boss
            {
                Id = 1,
                Title = "Первый босс",
                Armor = 5f,
                Hp = 100,
                MaxHp = 100,
                Attack = 15,
                StartAttack = 15,
                ExpReward = 100,
                GoldReward = 50,
                ImageResourcePath = "Boss/ebaka"
            };
            
            _gameService.Initialize(selectedCards, initialBoss);
            
            _cardViewModels.Clear();
            foreach (var card in selectedCards)
            {
                _cardViewModels.Add(new CardViewModel(card));
            }

            _bossViewModel = new BossViewModel(initialBoss);
            
            CurrentZone = _gameService.CurrentZone;
            UpdateTotalStats();
            
            ZoneChanged?.Invoke(CurrentZone);
            SendBattleMessage("Игра началась");
        }

        public void PerformAttackCycle()
        {
            if (_gameService.AreAllCardsDead())
            {
                SendBattleMessage("Карточки побеждены! Игра окончена.");
                GameOver?.Invoke();
                return;
            }

            var (effectiveDamage, bossDefeated) = _gameService.AttackBoss();
            SendBattleMessage($"Игрок наносит {effectiveDamage} урона боссу.");
            _bossViewModel.UpdateView(); // Обновление ViewModel босса

            if (bossDefeated)
            {
                SendBattleMessage("Босс побеждён! Переход к следующей зоне...");
                AdvanceToNextZone();
                return;
            }

            _gameService.BossAttack();
            SendBattleMessage($"Босс наносит {_gameService.CurrentBoss.Attack} урона каждой карточке.");
            
            foreach (var cardViewModel in _cardViewModels)
            {
                cardViewModel.UpdateView();
            }
            
            UpdateTotalStats();
        }

        private void AdvanceToNextZone()
        {
            _gameService.AdvanceToNextZone();
            
            CurrentZone = _gameService.CurrentZone;
            _bossViewModel.UpdateView();
            
            foreach (var cardViewModel in _cardViewModels)
            {
                cardViewModel.UpdateView();
            }
            
            UpdateTotalStats();
            
            ZoneChanged?.Invoke(CurrentZone);
            SendBattleMessage($"Зона {CurrentZone} началась. Новый босс: HP = {_bossViewModel.Hp}, Атака = {_bossViewModel.Attack}");
        }

        private void UpdateTotalStats()
        {
            TotalCardHp = _gameService.TotalCardStats.Hp;
            TotalCardAttack = _gameService.TotalCardStats.Attack;
            
            CardsTotalHpChanged?.Invoke(TotalCardHp);
            CardsTotalDamageChanged?.Invoke(TotalCardAttack);
        }

        private void SendBattleMessage(string message)
        {
            BattleMessage = message;
            BattleMessageSent?.Invoke(message);
        }
    }
}