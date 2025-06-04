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
            _model = model;
            _view  = view;
            
            _model.OnInventoryChanged += HandleInventoryChanged;
            _model.OnSelectionChanged += HandleInventoryChanged;
            HandleInventoryChanged();
        }

        private void HandleInventoryChanged()
        {
            _view.ClearAllItems();
            
            var toShow = _model.Items
                .OfType<CardModel>()
                .Where(c => !_model.SelectedItems.OfType<CardModel>().Contains(c))
                .ToList();

            foreach (var card in toShow)
            {
                var slot = _view.SpawnItemView();
                slot.Init(card, this, isToolbar: false);
                var view = slot.GetComponent<CardView>();
                new CardPresenter().Init(card, view);
                slot.OnDroppedInContainer(false);
            }
        }
        
        public bool IsSelected(CardModel card) 
            => _model.SelectedItems.OfType<CardModel>().Contains(card);

        public void AddOrStackCard(CardModel cardModel)
            => _model.AddOrStackItem(cardModel);

        public void Dispose()
            => _model.OnInventoryChanged -= HandleInventoryChanged;
        
        public void ToggleSelection(CardModel card)
            => _model.ToggleSelection(card);
    }
}