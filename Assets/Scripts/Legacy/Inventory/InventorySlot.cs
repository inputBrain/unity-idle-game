using UnityEngine;
using UnityEngine.EventSystems;

namespace OLd.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (this.transform.childCount == 0)
            {
                var inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = this.transform;
            }

        }
    }
}