using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Legacy.Battle;
using Presentation.MVP.Presenter;
using Presentation.MVP.Views;
using Presentation.Services;
using UnityEngine;

namespace Presentation
{
    public class GameEntryPoint : MonoBehaviour
    {
        private CardLoaderService _cardLoaderService;

        public TotalCardStatsView TotalCardStatsView;

        public List<CardView> CardsViews;
        public BossView BossView;

        public ZoneView ZoneView;
        

        private void Awake()
        {
            _cardLoaderService = new CardLoaderService();
            var cards = _cardLoaderService.GetDomainCards();
            var zone = new Zone();
            var boss = new Boss
            {
                Attack = 100f,
                MaxHp = 300f,
                Hp = 300,
                StartAttack = 100f,
                Id = 1,
                Title = "123"
            };

            InitUI(cards, boss, zone);
           
            
            var battleScript = new BattleScript(boss, zone, cards.Take(CardsViews.Count).ToList());

            StartCoroutine(StartBattleLoop(battleScript));

        }
        
        
        private IEnumerator StartBattleLoop(BattleScript battleScript)
        {
            while (true)
            {
                battleScript.BattleUpdate();
                
                yield return new WaitForSeconds(1f);
            }
        }


        private void InitUI(List<Card> cards, Boss boss, Zone zone)
        {
            new TotalCardStatsPresenter().Init(cards, TotalCardStatsView);
            new BossPresenter().Init(boss,BossView);
            
            for (var i = 0; i < CardsViews.Count; i++)
            {
                new CardPresenter().Init(cards[i], CardsViews[i]);
            }
            
            new ZonePresenter(ZoneView, zone);
        }

    }
}