using System;
using System.Collections.Generic;
using Api.Payload;
using Model.Card;
using Model.InventoryCard;
using Presentation.Entity;
using Presentation.Inventory;
using UnityEngine;

namespace Presentation.Toolbar
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private Transform toolbarContainerGrid;
        [SerializeField] private GameObject itemSlotPrefab;

        private readonly List<EntityView> _currentToolbarSlots = new();

        // На событие подписывается ToolbarPresenter
        public event Action<IInventoryItem> OnToolbarItemDropped;

        public void DisplayToolbarCards(IReadOnlyList<CardModel> cards)
        {
            ClearToolbar();

            foreach (var card in cards)
            {
                if (card == null) continue;

                GameObject slotInstance = Instantiate(itemSlotPrefab, toolbarContainerGrid);
                var itemView = slotInstance.GetComponent<EntityView>();
                if (itemView != null)
                {
                    var item = new EntityItem(card, LoadSprite(card.IconResourcesPath.Value));
                    itemView.Init(item, null, toolbarContainerGrid, isToolbar: true);
                    _currentToolbarSlots.Add(itemView);
                }
                else
                {
                    Debug.LogError("Префаб слота не содержит ItemView.");
                    Destroy(slotInstance);
                }
            }
        }

        private void ClearToolbar()
        {
            foreach (var slot in _currentToolbarSlots)
            {
                if (slot != null)
                    Destroy(slot.gameObject);
            }
            _currentToolbarSlots.Clear();
        }

        private void HandleToolbarSlotClick(IInventoryItem clickedItem)
        {
            OnToolbarItemDropped?.Invoke(clickedItem);
        }

        private Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return Resources.Load<Sprite>(path);
        }
    }
}