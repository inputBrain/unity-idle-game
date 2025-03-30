using System.Collections;
using System.Collections.Generic;
using Domain.Entities;
using Legacy.Battle;
using Presentation.MVP.Manager;
using Presentation.MVP.Presenter;
using Presentation.MVP.Views;
using Presentation.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class GameEntryPoint : MonoBehaviour
    {
        //Вьюхи
        public TotalCardStatsView totalCardStatsView;
        public BossView bossView;
        public ZoneView zoneView;

        //Менеджеры и сервисы
        public CardInventoryManager cardInventoryManager;
        private CardLoaderService _cardLoaderService;

        //Доменные сущности
        private Boss _boss;
        private Zone _zone;

        //UI
        public Button startGameButton;
        public Transform gameUI;
        public Transform cardsToolbarUI;
        public Transform inventoryUI;

        // Префабы
        public CardView cardViewPrefab;


        private void Awake()
        {
            _cardLoaderService = new CardLoaderService();
            var cards = _cardLoaderService.GetDomainCards();
            
            _zone = new Zone { CurrentZone = { Value = 1 } };

            _boss = new Boss
            {
                Attack = { Value = 100f },
                MaxHp = { Value = 300f },
                BaseHp = 300f,
                CurrentHp = { Value = 300f },
                Id = 1,
                Title = "123"
            };

            foreach (var c in cards)
            {
                cardInventoryManager.AddItem(c);
            }

            InitUI(cards, _boss, _zone);

            startGameButton.onClick.AddListener((() =>
            {
                startGameButton.gameObject.SetActive(false);
                inventoryUI.gameObject.SetActive(false);

                gameUI.gameObject.SetActive(true);
                cardsToolbarUI.gameObject.SetActive(true);

                InitUISelectedCards();
                StartBattle();
            }));
        }

        private void StartBattle()
        {
            if (cardInventoryManager.selectedItems.Count > 0)
            {
                var battleScript = new BattleScript(_boss, _zone, cardInventoryManager.selectedItems);
                StartCoroutine(StartBattleLoop(battleScript));
            }
            else
            {
                Debug.Log("Нет выбранных карточек для битвы.");
            }
        }

        private void InitUI(List<Card> cards, Boss boss, Zone zone)
        {
            new TotalCardStatsPresenter().Init(cards, totalCardStatsView);
            new BossPresenter().Init(boss, bossView);
            new ZonePresenter().Init(zone, zoneView);
        }
        private void InitUISelectedCards()
        {
            foreach (var card in cardInventoryManager.selectedItems)
            {
                CardView newCardView = Instantiate(cardViewPrefab, cardsToolbarUI);
                new CardPresenter().Init(card, newCardView);
            }
        }

        private IEnumerator StartBattleLoop(BattleScript battleScript)
        {
            while (true)
            {
                battleScript.BattleUpdate();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}