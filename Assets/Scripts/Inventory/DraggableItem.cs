using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image image;
        
        [HideInInspector]
        public Transform parentAfterDrag;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag drag");
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsFirstSibling();
            image.raycastTarget = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag drag");
            transform.position = Input.mousePosition;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag drag");
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
}
