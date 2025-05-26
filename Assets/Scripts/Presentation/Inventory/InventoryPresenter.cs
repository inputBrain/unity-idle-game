using System;
using System.Linq;
using Model.Card;
using Model.Inventory;
using Presentation.Card;

namespace Presentation.Inventory
{
    public class InventoryPresenter : IDisposable
    {
        private readonly InventoryModel   _model;
        private readonly InventoryView    _view;

        public InventoryPresenter(InventoryModel model, InventoryView view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _view  = view  ?? throw new ArgumentNullException(nameof(view));

            _model.OnInventoryChanged += HandleInventoryChanged;
            HandleInventoryChanged();
        }

        private void HandleInventoryChanged()
        {
            _view.ClearAllItems();
            foreach (var card in _model.Items.OfType<CardModel>())
            {
                var slot = _view.SpawnItemView();
                slot.Init(card, this, isToolbar: false);

                var cardView = slot.GetComponent<CardView>();
                new CardPresenter().Init(card, cardView);
            }
        }

        public void AddOrStackCard(CardModel cardModel)
            => _model.AddOrStackItem(cardModel);

        public void Dispose()
            => _model.OnInventoryChanged -= HandleInventoryChanged;
        
        public void ToggleSelection(CardModel card)
            => _model.ToggleSelection(card);
    }
}