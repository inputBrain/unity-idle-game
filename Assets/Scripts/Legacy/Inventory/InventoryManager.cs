using ScriptableObjects.Config;
using UnityEngine;

namespace OLd.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public InventorySlot[] InventorySlots;
        public GameObject inventoryItemPrefab;
        
        public void AddItem(CardObject item)
        {
            for (var i = 0; i < InventorySlots.Length; i++)
            {
                var slot = InventorySlots[i];
                var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    return;
                }
            }
        }


        public void SpawnNewItem(CardObject item, InventorySlot slot)
        {
            var newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
            var inventoryItem = newItemGo.GetComponent<InventoryItem>();
            inventoryItem.InitItem(item);
        }
    }
}