using Inventory;
using ScriptableObjects;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
        public InventoryManager inventoryManager;

        public CardObject[] itemsToPickup;


        public void PickupItem(int id)
        {
                inventoryManager.AddItem(itemsToPickup[id]);
        }
}