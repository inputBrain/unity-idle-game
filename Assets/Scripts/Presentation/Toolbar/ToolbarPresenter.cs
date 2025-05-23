using System;
using System.Collections.Generic;
using Model.Card;
using Model.InventoryCard;
using Presentation.TotalStats;

namespace Presentation.Toolbar
{
    public class ToolbarPresenter
    {
        private readonly ToolbarView _toolbarView;
        private readonly List<CardModel> _toolbarCards = new();
        private readonly TotalCardStatsPresenter _statsPresenter;
        private readonly TotalCardStatsView _statsView;

        private const int MaxToolbarCards = 5;

        public ToolbarPresenter(ToolbarView toolbarView, TotalCardStatsView statsView)
        {
            _toolbarView = toolbarView ?? throw new ArgumentNullException(nameof(toolbarView));
            _statsView = statsView ?? throw new ArgumentNullException(nameof(statsView));
            _statsPresenter = new TotalCardStatsPresenter();
        }

        public void AddCard(CardModel cardModel)
        {
            if (cardModel == null) return;
            if (_toolbarCards.Count >= MaxToolbarCards) return;
            if (_toolbarCards.Contains(cardModel)) return;

            _toolbarCards.Add(cardModel);
            UpdateToolbarView();
        }

        public void RemoveCard(CardModel cardModel)
        {
            if (cardModel == null) return;
            _toolbarCards.Remove(cardModel);
            UpdateToolbarView();
        }

        private void HandleItemDropped(IInventoryItem domainItem)
        {
            if (domainItem is CardModel card)
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
            _statsPresenter.Init(_toolbarCards, _statsView);
        }

        public IReadOnlyList<CardModel> GetCards() => _toolbarCards;
    }
}
