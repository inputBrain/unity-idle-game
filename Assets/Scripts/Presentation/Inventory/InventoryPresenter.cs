using System;
using System.Collections.Generic;
using System.Linq;
using Model.Card;
using Model.Inventory;
using Model.InventoryCard;
using UnityEngine;
using Utils;
using Presentation.Toolbar;

namespace Presentation.Inventory
{
    public class InventoryPresenter : Singleton<InventoryPresenter>, IDisposable
    {
        private readonly InventoryModel _inventoryModel;
        private readonly Dictionary<string, Sprite> _spriteCache = new();

        private readonly Sprite _defaultIcon;

        private ToolbarPresenter _toolbarPresenter;

        public InventoryPresenter(InventoryModel model, InventoryView view, Sprite defaultIcon = null)
        {
            _inventoryModel = model ?? throw new ArgumentNullException(nameof(model));
            _defaultIcon = defaultIcon;

            _inventoryModel.OnInventoryChanged += HandleInventoryChanged;
            HandleInventoryChanged();
        }

        private void HandleInventoryChanged()
        {
            var itemsToDisplay = _inventoryModel.Items
                .Where(domainItem => domainItem != null)
                .Select(domainItem => {
                    var iconPath = domainItem.IconResourcesPath.Value;
                    var icon = LoadSprite(iconPath);
                    return new EntityItem(domainItem, icon);
                }).ToList();
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
            _spriteCache.Clear();
        }

        public void AddOrStackCard(CardModel cardModel)
        {
            _inventoryModel.AddOrStackItem(cardModel);
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
    }
}