using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
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
        [SerializeField]
        public List<Card> Cards;
        [SerializeField]
        public float CardHps;

        

        private void Awake()
        {
            _cardLoaderService = new CardLoaderService();
            Cards = _cardLoaderService.GetDomainCards();

            new TotalCardStatsPresenter(Cards, TotalCardStatsView);
            InvokeRepeating(nameof(AddHp), 1f, 1f);
        }


        void AddHp()
        {
            foreach (var card in Cards)
            {
                card.CurrentHp += 1;
            }
            CardHps = Cards.Sum(c => c.CurrentHp);

        }
    }
}