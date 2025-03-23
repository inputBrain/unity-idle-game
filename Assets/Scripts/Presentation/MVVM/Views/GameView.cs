using Presentation.MVVM.ViewModels;
using Presentation.Services;
using TMPro;
using UnityEngine;

namespace Presentation.MVVM.Views
{
    public class GameView : MonoBehaviour
    {
        public CardView[] cardViews;
        public BossView bossView;
        public TMP_Text cardsHpText;
        public TMP_Text cardsDamageText;
        public TMP_Text zoneText;
        public TMP_Text battleLogText;

        private GameViewModel _gameViewModel;
        private CardLoaderService _cardLoaderService;

        void Awake()
        {
            _cardLoaderService = new CardLoaderService();
            _gameViewModel = new GameViewModel(_cardLoaderService);
            
            _gameViewModel.CardsTotalHpChanged += UpdateCardHp;
            _gameViewModel.CardsTotalDamageChanged += UpdateCardDamage;
            _gameViewModel.ZoneChanged += UpdateZone;
            _gameViewModel.BattleMessageSent += UpdateBattleLog;
            _gameViewModel.GameOver += OnGameOver;
            
            InitializeGame();
            this.InvokeRepeating(nameof(PerformAttackCycle), 1f, 1f);
        }

        void OnDestroy()
        {
            if (_gameViewModel != null)
            {
                _gameViewModel.CardsTotalHpChanged -= UpdateCardHp;
                _gameViewModel.CardsTotalDamageChanged -= UpdateCardDamage;
                _gameViewModel.ZoneChanged -= UpdateZone;
                _gameViewModel.BattleMessageSent -= UpdateBattleLog;
                _gameViewModel.GameOver -= OnGameOver;
            }
            
            if (this.IsInvoking(nameof(PerformAttackCycle)))
                this.CancelInvoke(nameof(PerformAttackCycle));
        }

        void InitializeGame()
        {
            _gameViewModel.InitializeGame(cardViews.Length);
            
            var cardViewModels = _gameViewModel.CardViewModels;
            for (int i = 0; i < cardViews.Length && i < cardViewModels.Count; i++)
            {
                cardViews[i].Initialize(cardViewModels[i]);
            }
            
            bossView.Initialize(_gameViewModel.CurrentBoss);
        }

        void PerformAttackCycle()
        {
            _gameViewModel.PerformAttackCycle();
        }

        void UpdateCardHp(float newHp)
        {
            if (cardsHpText != null)
            {
                cardsHpText.text = "Total HP: " + newHp;
            }
        }

        void UpdateCardDamage(float newDamage)
        {
            if (cardsDamageText != null)
            {
                cardsDamageText.text = "Total attack: " + newDamage;
            }
        }

        void UpdateZone(int zone)
        {
            if (zoneText != null)
            {
                zoneText.text = "Zone: " + zone;
            }
        }

        void UpdateBattleLog(string message)
        {
            if (battleLogText != null)
            {
                battleLogText.text = message;
            }
        }
        
        void OnGameOver()
        {
            this.CancelInvoke(nameof(PerformAttackCycle));
        }
    }
}