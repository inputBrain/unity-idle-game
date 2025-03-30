using System.Collections.Generic;
using Domain;
using Presentation.MVP.Presenter;
using UnityEngine;

namespace Presentation.MVP.Manager
{
    public interface IInventoryManager
    {
        void SelectItem(BaseEntity item);
        void RemoveItem(BaseEntity item);
    }

    public abstract class BaseInventoryManager<T> : MonoBehaviour where T : BaseEntity
    {
        [SerializeField] protected Transform inventoryContainer;
        [SerializeField] protected GameObject inventoryItemPrefab;

        private List<T> _items = new();
        private List<InventoryItemPresenter> _inventoryItems = new();
        public List<T> selectedItems = new();

        public void AddItem(T item)
        {
            _items.Add(item);
            CreateInventoryItemOnScene(item);
        }

        protected void RemoveItem(T item)
        {
            int index = _items.IndexOf(item);
            if (index < 0 || index >= _inventoryItems.Count) return;

            // Удаляем GO со сцены, сам объект и его презентер
            Destroy(_inventoryItems[index].gameObject);
            _inventoryItems.RemoveAt(index);
            _items.Remove(item);
        }

        protected void SelectItem(T item)
        {
            if (!selectedItems.Contains(item))
            {
                selectedItems.Add(item);
                Debug.Log($"Item {item.Title}, ID {item.Id} выбран.");
            }
            else
            {
                Debug.Log($"Item {item.Title}, ID {item.Id} уже выбран.");
            }
        }

        protected virtual void CreateInventoryItemOnScene(T item)
        {
            GameObject inventoryGo = Instantiate(inventoryItemPrefab, inventoryContainer);
            InventoryItem inventoryItem = inventoryGo.GetComponent<InventoryItem>();
            InventoryItemPresenter presenter = inventoryGo.AddComponent<InventoryItemPresenter>();
            presenter.Init(item, inventoryItem, this as IInventoryManager);
            _inventoryItems.Add(presenter);
        }
    }
}