using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Domain.Interfaces; 
using UnityEngine;

namespace Presentation.MVP.Views
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform itemSlotsContainer;
        [SerializeField] private GameObject itemSlotPrefab;

        private readonly List<ItemView> _currentSlots = new();
        
        // На ивенты подписывается презентер
        public event Action<IInventoryItem> OnItemClicked;
        public event Action<IInventoryItem> OnItemDeleteClicked; 

        public void DisplayItems(IReadOnlyList<Item> itemsToDisplay)
        {
            foreach (var slot in _currentSlots.Where(slot => slot != null))
            {
                Destroy(slot.gameObject);
            }
            _currentSlots.Clear();
            

            if (itemSlotPrefab == null || itemSlotsContainer == null)
            {
                 Debug.LogError("InventoryView: отсутствует префаб слота или контейнер.");
                 return;
            }

            if (itemsToDisplay == null) return;

            foreach (var item in itemsToDisplay) 
            {
                 if (item == null) continue;

                //Из списка Item создаем GO на сцене из префаба
                GameObject slotInstance = Instantiate(itemSlotPrefab, itemSlotsContainer);
                ItemView itemView = slotInstance.GetComponent<ItemView>();
                if (itemView != null)
                {
                    //Если на префабе есть вьюха, то в нее передаем item и  Хандлеры для обработки по типу Drag/Drop/Click
                    itemView.Init(item, HandleSlotClick, HandleDeleteClick);
                    _currentSlots.Add(itemView);
                }
                else
                {
                    Debug.LogError("Префаб слота не содержит ItemSlotView.");
                    Destroy(slotInstance);
                }
            }
        }

        public void UpdateSelection(HashSet<IInventoryItem> selectedItems)
        {
            foreach (var slot in _currentSlots)
            {
                if (slot != null && slot.DomainItem != null)
                {
                    slot.SetSelected(selectedItems.Contains(slot.DomainItem));
                }
                else if (slot != null)
                {
                    slot.SetSelected(false);
                }
            }
        }


        private void HandleSlotClick(IInventoryItem clickedItem)
        {
            OnItemClicked?.Invoke(clickedItem);
        }

        private void HandleDeleteClick(IInventoryItem itemToDelete) 
        {
            OnItemDeleteClicked?.Invoke(itemToDelete);
        }
    }
}