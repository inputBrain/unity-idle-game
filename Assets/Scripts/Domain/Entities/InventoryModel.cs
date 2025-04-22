using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using UnityEngine;

namespace Domain.Entities
{
    public class InventoryModel
    {
        public List<IInventoryItem> Items { get; } = new();
        
        //HashSet, чтобы Contains был O(1)
        public HashSet<IInventoryItem> SelectedItems { get; } = new();

        public event Action OnInventoryChanged;
        public event Action OnSelectionChanged;


        public void AddItem(IInventoryItem domainItem)
        {
            if (domainItem == null || Items.Contains(domainItem)) return;
            Items.Add(domainItem);
            //Добавили в модель item? Уведомили подписчиков
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(IInventoryItem domainItem)
        {
            if (domainItem == null) return;

            var removed = Items.Remove(domainItem);
            if (removed)
            {
                var selectionChanged = SelectedItems.Remove(domainItem);
                OnInventoryChanged?.Invoke();
                if (selectionChanged)
                {
                    OnSelectionChanged?.Invoke();
                }
            }
        }

        public void ToggleSelection(IInventoryItem itemToToggle)
        {
            if (itemToToggle == null || !Items.Contains(itemToToggle)) return;
            
            Debug.LogWarning($"InventoryModel: Выбрал {itemToToggle.Title}");

            if (!SelectedItems.Add(itemToToggle))
            {
                SelectedItems.Remove(itemToToggle);
            }
            OnSelectionChanged?.Invoke();
        }

        public void ClearSelection()
        {
            if (SelectedItems.Count <= 0) return;
            SelectedItems.Clear();
            OnSelectionChanged?.Invoke();
        }

        public void LoadItems(IEnumerable<IInventoryItem> domainItems)
        {
             if (domainItems == null) return;
             Items.Clear();
             Items.AddRange(domainItems.Where(item => item != null));
            
            OnInventoryChanged?.Invoke();
            
            if (SelectedItems.Count > 0)
            {
                SelectedItems.Clear();
                OnSelectionChanged?.Invoke();
            }
        }
    }
}