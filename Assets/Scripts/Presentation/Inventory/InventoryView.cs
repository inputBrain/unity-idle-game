using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Domain.Interfaces;
using UnityEngine;

namespace Presentation.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform inventoryContainerGrid;
        [SerializeField] private GameObject itemSlotPrefab;

        private readonly List<ItemView> _currentSlots = new();
        
        // На ивенты подписывается презентер
        public event Action<IInventoryItem> OnItemClicked;
        public event Action<IInventoryItem> OnItemDeleteClicked; 
        public event Action<IInventoryItem> OnToolbarSelected; 

        public void DisplayItems(IReadOnlyList<Item> itemsToDisplay)
        {
            foreach (var slot in _currentSlots.Where(slot => slot != null))
            {
                Destroy(slot.gameObject);
            }
            _currentSlots.Clear();
            

            if (itemSlotPrefab == null || inventoryContainerGrid == null)
            {
                 Debug.LogError("InventoryView: отсутствует префаб слота или контейнер.");
                 return;
            }

            if (itemsToDisplay == null) return;

            foreach (var item in itemsToDisplay) 
            {
                 if (item == null) continue;

                //Из списка Item создаем GO на сцене из префаба
                GameObject slotInstance = Instantiate(itemSlotPrefab, inventoryContainerGrid);
                ItemView itemView = slotInstance.GetComponent<ItemView>();
                if (itemView != null)
                {
                    //Если на префабе есть вьюха, то в нее передаем item и  Хандлеры для обработки по типу Drag/Drop/Click
                    itemView.Init(item, HandleSlotClick);
                    _currentSlots.Add(itemView);
                }
                else
                {
                    Debug.LogError("Префаб слота не содержит ItemSlotView.");
                    Destroy(slotInstance);
                }
            }
        }
        


        private void HandleSlotClick(IInventoryItem clickedItem)
        {
            OnItemClicked?.Invoke(clickedItem);
        }
    }
}