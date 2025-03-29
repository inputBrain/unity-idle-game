using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using OLd.Battle;
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

        public ZoneView ZoneView;
        

        private async void Awake()
        {
            _cardLoaderService = new CardLoaderService();
            var cards = _cardLoaderService.GetDomainCards();
            var zone = new Zone();
            var boss = new Boss();
            
            new TotalCardStatsPresenter(cards, TotalCardStatsView);

            
            for (var i = 0; i < CardsViews.Count; i++)
            {
                var cardPresenter = gameObject.AddComponent<CardPresenter>();
                cardPresenter.Init(cards[i], CardsViews[i]);
            }
            
            
            gameObject.AddComponent<ZonePresenter>();
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
    }
}