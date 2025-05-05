using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Domain.Entities;
using Domain.Interfaces;
using UnityEngine;
using Utils;
using Presentation.Toolbar;

namespace Presentation.Inventory
{
    public class InventoryPresenter : Singleton<InventoryPresenter>, IDisposable
    {
        private readonly InventoryModel _inventoryModel;
        private readonly InventoryView _inventoryView;
        private readonly Dictionary<string, Sprite> _spriteCache = new();

        private readonly Sprite _defaultIcon;

        private ToolbarPresenter _toolbarPresenter;

        public InventoryPresenter(InventoryModel model, InventoryView view, Sprite defaultIcon = null)
        {
            _inventoryModel = model ?? throw new ArgumentNullException(nameof(model));
            _inventoryView = view ?? throw new ArgumentNullException(nameof(view));
            _defaultIcon = defaultIcon;

            _inventoryModel.OnInventoryChanged += HandleInventoryChanged;
            _inventoryView.OnItemClicked += HandleItemClicked;
            _inventoryView.OnItemDeleteClicked += HandleItemDeleteClicked;

            HandleInventoryChanged();
        }

        public void SetToolbarPresenter(ToolbarPresenter toolbarPresenter)
        {
            _toolbarPresenter = toolbarPresenter;
        }

        private void HandleInventoryChanged()
        {
            var itemsToDisplay = _inventoryModel.Items
                .Where(domainItem => domainItem != null)
                .Select(domainItem => {
                    var iconPath = domainItem.IconResourcesPath.Value;
                    var icon = LoadSprite(iconPath);
                    return new Item(domainItem, icon);
                }).ToList();

            _inventoryView.DisplayItems(itemsToDisplay);
        }

        private void HandleItemClicked(IInventoryItem clickedItem)
        {
            Debug.Log($"InventoryPresenter: Кликнул: {clickedItem.Title}");
            _inventoryModel.ToggleSelection(clickedItem);
        }

        private void HandleItemDeleteClicked(IInventoryItem itemToDelete)
        {
            Debug.Log($"InventoryPresenter: Запрос на удаление: {itemToDelete.Title}");
            _inventoryModel.RemoveItem(itemToDelete);
        }

        private Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path)) return _defaultIcon;
            if (_spriteCache.TryGetValue(path, out var cached)) return cached;

            var loaded = Resources.Load<Sprite>(path);
            if (loaded == null)
            {
                Debug.LogWarning($"Sprite not found at Resources/{path}");
                loaded = _defaultIcon;
            }

            _spriteCache[path] = loaded;
            return loaded;
        }

        public void Dispose()
        {
            if (_inventoryModel != null)
                _inventoryModel.OnInventoryChanged -= HandleInventoryChanged;
            if (_inventoryView != null)
            {
                _inventoryView.OnItemClicked -= HandleItemClicked;
                _inventoryView.OnItemDeleteClicked -= HandleItemDeleteClicked;
            }
            _spriteCache.Clear();
        }

        public void AddOrStackCard(Domain.Entities.Card card)
        {
            _inventoryModel.AddOrStackItem(card);
        }

        public void AddItemToInventory(IInventoryItem domainItem)
        {
            _inventoryModel.AddItem(domainItem);
        }

        public void RemoveItemFromInventory(IInventoryItem domainItem)
        {
            _inventoryModel.RemoveItem(domainItem);
        }

        public IEnumerable<IInventoryItem> GetSelectedDomainItems() => _inventoryModel.SelectedItems;
        public void ClearInventorySelection() => _inventoryModel.ClearSelection();

        public bool TryTransferToToolbar(Domain.Entities.Card card)
        {
            if (_toolbarPresenter == null)
            {
                Debug.LogWarning("InventoryPresenter: ToolbarPresenter не задан.");
                return false;
            }

            var alreadyInToolbar = _toolbarPresenter.GetCards().Any(c => c.Id == card.Id);
            if (alreadyInToolbar)
            {
                Debug.Log("InventoryPresenter: Карта уже в тулбаре.");
                return false;
            }

            if (_toolbarPresenter.GetCards().Count >= 5)
            {
                Debug.Log("InventoryPresenter: Тулбар заполнен.");
                return false;
            }

            RemoveItemFromInventory(card);
            _toolbarPresenter.AddCard(card);
            return true;
        }

        public void ReturnCardToInventory(Domain.Entities.Card card)
        {
            AddOrStackCard(card);
        }
    }
}