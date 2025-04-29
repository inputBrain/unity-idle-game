using System;
using System.Collections.Generic;
using Domain.Entities;
using Presentation.TotalStats;
using UnityEngine;

namespace Presentation.Toolbar
{
    public class ToolbarPresenter
    {
        private readonly ToolbarView _view;
        private readonly TotalCardStatsPresenter _statsPresenter;

        private readonly List<Domain.Entities.Card> _toolbarCards = new();

        public ToolbarPresenter(ToolbarView view, TotalCardStatsView statsView)
        {
            _view = view;
            _statsPresenter = new TotalCardStatsPresenter();
            _view.OnToolbarCardsChanged += UpdateStats;
            _statsPresenter.Init(_toolbarCards, statsView);
        }

        public void AddCard(Domain.Entities.Card card)
        {
            if (_toolbarCards.Count >= 5)
            {
                Debug.Log("ToolbarPresenter: максимум 5 карт.");
                return;
            }

            _toolbarCards.Add(card);
            _view.DisplayToolbarCards(_toolbarCards);
        }

        public void RemoveCard(Domain.Entities.Card card)
        {
            _toolbarCards.Remove(card);
            _view.DisplayToolbarCards(_toolbarCards);
        }

        private void UpdateStats(List<Domain.Entities.Card> currentCards)
        {
            _statsPresenter.Init(currentCards, _statsPresenter.View);
        }

        public List<Domain.Entities.Card> GetCards() => _toolbarCards;
    }
}