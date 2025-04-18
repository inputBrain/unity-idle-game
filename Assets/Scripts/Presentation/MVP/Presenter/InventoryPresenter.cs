using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class InventoryPresenter : IDisposable
    {
        private readonly InventoryModel _inventoryModel;
        private readonly InventoryView _inventoryView;
        private readonly Dictionary<string, Sprite> _spriteCache = new();
        
        private readonly Sprite _defaultIcon;

        public InventoryPresenter(InventoryModel model, InventoryView view, Sprite defaultIcon = null)
        {
            _inventoryModel = model ?? throw new ArgumentNullException(nameof(model));
            _inventoryView = view ?? throw new ArgumentNullException(nameof(view));
            _defaultIcon = defaultIcon;

            // Поменялась модель инвентаря в BLL? Обновляем UI
            _inventoryModel.OnInventoryChanged += HandleInventoryChanged;
            
            //Что то произошло в UI? Обновляем модель инвентаря
            _inventoryView.OnItemClicked += HandleItemClicked;
            _inventoryView.OnItemDeleteClicked += HandleItemDeleteClicked;

            //Чисто для первой инициализации вызываем
            HandleInventoryChanged();
        }

        #region Обработчики Model => UI

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
        

        #endregion

        #region Обработчики UI => model
        // Обработчик клика по слоту
        private void HandleItemClicked(IInventoryItem clickedItem)
        {
            Debug.Log($"InventoryPresenter: Кликнул: {clickedItem.Title}");
            _inventoryModel.ToggleSelection(clickedItem);
        }

         // Обработчик клика по кнопке удаления
        private void HandleItemDeleteClicked(IInventoryItem itemToDelete)
        {
             Debug.Log($"InventoryPresenter: Запрос на удаление: {itemToDelete.Title}");
            _inventoryModel.RemoveItem(itemToDelete);
        }
        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Загружаем картинки по пути и сохраняем в кэш
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
            {
                _inventoryModel.OnInventoryChanged -= HandleInventoryChanged;
            }
            if (_inventoryView != null)
            {
                _inventoryView.OnItemClicked -= HandleItemClicked;
                _inventoryView.OnItemDeleteClicked -= HandleItemDeleteClicked;
            }
            _spriteCache.Clear();
        }

        #endregion
        
        #region Публичные методы для управления из BLL
        public void AddItemToInventory(IInventoryItem domainItem) => _inventoryModel.AddItem(domainItem);
        public void RemoveItemFromInventory(IInventoryItem domainItem) => _inventoryModel.RemoveItem(domainItem);
        public IEnumerable<IInventoryItem> GetSelectedDomainItems() => _inventoryModel.SelectedItems;
        public void ClearInventorySelection() => _inventoryModel.ClearSelection();
        #endregion
    }
}