using System;
using System.Collections.Generic;
using Presentation.TotalStats;

namespace Presentation.Toolbar
{
    public class ToolbarPresenter
    {
        private readonly ToolbarView _toolbarView;
        private readonly List<Domain.Entities.Card> _toolbarCards = new();
        private readonly TotalCardStatsPresenter _statsPresenter;
        private readonly TotalCardStatsView _statsView;

        private const int MaxToolbarCards = 5;

        public ToolbarPresenter(ToolbarView toolbarView, TotalCardStatsView statsView)
        {
            _toolbarView = toolbarView ?? throw new ArgumentNullException(nameof(toolbarView));
            _statsView = statsView ?? throw new ArgumentNullException(nameof(statsView));
            _statsPresenter = new TotalCardStatsPresenter();

            _toolbarView.OnToolbarItemDropped += HandleItemDropped;
        }

        public void AddCard(Domain.Entities.Card card)
        {
            if (card == null) return;
            if (_toolbarCards.Count >= MaxToolbarCards) return;
            if (_toolbarCards.Contains(card)) return;

            _toolbarCards.Add(card);
            UpdateToolbarView();
        }

        public void RemoveCard(Domain.Entities.Card card)
        {
            if (card == null) return;
            _toolbarCards.Remove(card);
            UpdateToolbarView();
        }

        private void HandleItemDropped(Domain.Interfaces.IInventoryItem domainItem)
        {
            if (domainItem is Domain.Entities.Card card)
            {
                if (_toolbarCards.Contains(card))
                {
                    RemoveCard(card);
                }
                else
                {
                    AddCard(card);
                }
            }
        }

        private void UpdateToolbarView()
        {
            _toolbarView.DisplayToolbarCards(_toolbarCards);
            _statsPresenter.Init(_toolbarCards, _statsView);
        }

        public IReadOnlyList<Domain.Entities.Card> GetCards() => _toolbarCards;
    }
}
