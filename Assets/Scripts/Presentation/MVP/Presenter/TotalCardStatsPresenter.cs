using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class TotalCardStatsPresenter : MonoBehaviour
    {
        private readonly TotalCardStatsView _view;
        
        private readonly List<Card> _cards;


        public TotalCardStatsPresenter(List<Card> cards, TotalCardStatsView view)
        {
            _cards = cards;
            _view = view;

            UpdateTotalHp();

            foreach (var card in _cards)
            {
                card.OnCurrentHpChanged += _ => UpdateTotalHp();
            }
        }
        
        private void UpdateTotalHp()
        {
            var totalHp = _cards.Sum(card => card.CurrentHp);
            _view.SetHp(totalHp);
        }
    }
}