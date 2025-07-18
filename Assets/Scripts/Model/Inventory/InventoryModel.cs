﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model.Card;
using Model.InventoryCard;
using UnityEngine;

namespace Model.Inventory
{
    public class InventoryModel
    {
        public List<IInventoryItem> Items { get; } = new();
        public HashSet<IInventoryItem> SelectedItems { get; } = new();

        public event Action OnInventoryChanged;
        public event Action OnSelectionChanged;

        private void RaiseInventoryChanged() => OnInventoryChanged?.Invoke();
        private void RaiseSelectionChanged() => OnSelectionChanged?.Invoke();

        public void AddItem(IInventoryItem domainItem)
        {
            if (domainItem == null || Items.Contains(domainItem)) return;

            Items.Add(domainItem);
            RaiseInventoryChanged();
        }


        public void RemoveItem(IInventoryItem domainItem)
        {
            if (domainItem == null) return;

            if (Items.Remove(domainItem))
            {
                bool wasSelected = SelectedItems.Remove(domainItem);
                RaiseInventoryChanged();
                if (wasSelected) RaiseSelectionChanged();
            }
        }


        public void ToggleSelection(IInventoryItem itemToToggle)
        {
            if (itemToToggle == null || !Items.Contains(itemToToggle)) return;

            if (!SelectedItems.Add(itemToToggle))
                SelectedItems.Remove(itemToToggle);

            RaiseSelectionChanged();
        }


        public void ClearSelection()
        {
            if (SelectedItems.Count == 0) return;

            SelectedItems.Clear();
            RaiseSelectionChanged();
        }


        public void LoadItems(IEnumerable<IInventoryItem> domainItems)
        {
            if (domainItems == null) return;

            Items.Clear();
            Items.AddRange(domainItems.Where(item => item != null));
            RaiseInventoryChanged();

            if (SelectedItems.Count > 0)
            {
                SelectedItems.Clear();
                RaiseSelectionChanged();
            }
        }


        public void AddOrStackItem(IInventoryItem domainItem)
        {
            const int maxRank = CardModel.MaxRank;

            if (domainItem is not CardModel newCard || newCard.Count.Value <= 0)
                return;
            
            var existingCard = Items.OfType<CardModel>().FirstOrDefault(c => c.Id == newCard.Id);

            if (existingCard != null)
            {
                existingCard.Count.Value += 1;

                if (existingCard.Rank.Value < maxRank)
                {
                    var requiredForNextRank = (decimal)Math.Pow(2, existingCard.Rank.Value);

                    if (existingCard.Count.Value >= requiredForNextRank)
                    {
                        existingCard.Rank.Value += 1;
                        existingCard.Count.Value = 1;
                    }
                }

                RaiseInventoryChanged();
            }
            else
            {
                newCard.Rank.Value = 1;
                newCard.Count.Value = 1;
                AddItem(newCard);
            }

            OnInventoryChanged?.Invoke();
        }
    }
}
