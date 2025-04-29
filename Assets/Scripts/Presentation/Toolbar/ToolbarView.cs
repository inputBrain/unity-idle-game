using System;
using System.Collections.Generic;
using Application.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Presentation.Card;
using Presentation.Inventory;
using UnityEngine;

namespace Presentation.Toolbar
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private Transform toolbarContainerGrid;
        [SerializeField] private GameObject itemSlotPrefab;

        private readonly List<ItemView> _currentSlots = new();

        public event Action<List<Domain.Entities.Card>> OnToolbarCardsChanged;

        public void DisplayToolbarCards(List<Domain.Entities.Card> cards)
        {
            foreach (var slot in _currentSlots)
            {
                if (slot != null)
                    Destroy(slot.gameObject);
            }
            _currentSlots.Clear();

            if (toolbarContainerGrid == null || itemSlotPrefab == null)
            {
                Debug.LogError("ToolbarView: отсутствует контейнер или префаб.");
                return;
            }

            foreach (var card in cards)
            {
                var slot = Instantiate(itemSlotPrefab, toolbarContainerGrid);
                var itemView = slot.GetComponent<ItemView>();

                if (itemView != null)
                {
                    var item = new Item(card, Resources.Load<Sprite>(card.IconResourcesPath.Value));
                    itemView.Init(item, _ => { }, null, true);

                    var presenter = new CardPresenter();
                    presenter.Init(card, slot.GetComponent<CardView>());

                    _currentSlots.Add(itemView);
                }
            }

            OnToolbarCardsChanged?.Invoke(cards);
        }

        public List<Domain.Entities.Card> GetToolbarCards()
        {
            var result = new List<Domain.Entities.Card>();
            foreach (var slot in _currentSlots)
            {
                if (slot?.DomainItem is Domain.Entities.Card card)
                {
                    result.Add(card);
                }
            }
            return result;
        }
    }
}