using Domain;
using Presentation.MVP.Manager;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class InventoryItemPresenter : MonoBehaviour
    {
        private BaseEntity _item;
        private InventoryItem _inventoryItemView;
        private IInventoryManager _inventoryManager;

        public void Init(BaseEntity item, InventoryItem inventoryItemView, IInventoryManager inventoryManager)
        {
            _item = item;
            _inventoryItemView = inventoryItemView;
            _inventoryManager = inventoryManager;

            UpdateImage(_item.ImageResourcesPath.Value);
            
            // Подписываемся на события
            _inventoryItemView.OnItemClicked += HandleItemClicked;
            _inventoryItemView.OnItemDeleted += HandleItemDeleted;
        }

        private void HandleItemClicked(InventoryItem item)
        {
            Debug.Log($"Item с ID: {_item.Id} выбран.");
            _inventoryManager.SelectItem(_item);
        }
        
        private void HandleItemDeleted(InventoryItem item)
        {
            _inventoryManager.RemoveItem(_item);
        }
        
        private void UpdateImage(string path)
        {
            var newSprite = Resources.Load<Sprite>(path);
            if (newSprite != null)
            {
                _inventoryItemView.InitItem(newSprite);
            }
        }
        
        private void OnDestroy()
        {
            if (_inventoryItemView != null)
            {
                _inventoryItemView.OnItemClicked -= HandleItemClicked;
                _inventoryItemView.OnItemDeleted -= HandleItemDeleted;
            }
        }
    }
}
