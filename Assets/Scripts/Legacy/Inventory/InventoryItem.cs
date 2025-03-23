using ScriptableObjects.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OLd.Inventory
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector]
        public CardObject item;
        
        [Header("UI")]
        public Image image;
        
        [HideInInspector]
        public Transform parentAfterDrag;


        public void InitItem(CardObject newItem)
        {
            item = newItem;
            image.sprite = newItem.Image;
        }
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = this.transform.parent;
            this.transform.SetParent(this.transform.root);
            // transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            this.transform.SetParent(parentAfterDrag);
        }
    }
}