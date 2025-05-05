using System;
using System.Collections.Generic;
using Application.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Presentation.Inventory;
using UnityEngine;

namespace Presentation.Toolbar
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private Transform toolbarContainerGrid;
        [SerializeField] private GameObject itemSlotPrefab;

        private readonly List<ItemView> _currentToolbarSlots = new();

        // На событие подписывается ToolbarPresenter
        public event Action<IInventoryItem> OnToolbarItemDropped;

        public void DisplayToolbarCards(IReadOnlyList<Domain.Entities.Card> cards)
        {
            ClearToolbar();

            foreach (var card in cards)
            {
                if (card == null) continue;

                GameObject slotInstance = Instantiate(itemSlotPrefab, toolbarContainerGrid);
                var itemView = slotInstance.GetComponent<ItemView>();
                if (itemView != null)
                {
                    var item = new Item(card, LoadSprite(card.IconResourcesPath.Value));
                    itemView.Init(item, HandleToolbarSlotClick, new RectTransform(), isToolbar: true);
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