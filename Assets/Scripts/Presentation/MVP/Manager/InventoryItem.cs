using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Presentation.MVP.Manager
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public Image image;
        public Transform parentAfterDrag;
        public Button deleteCardButton;
        
        public event Action<InventoryItem> OnItemClicked;
        public event Action<InventoryItem> OnItemDeleted;

        public void InitItem(Sprite sprite)
        {
            if (sprite != null)
            {
                image.sprite = sprite;
            }

            deleteCardButton.onClick.AddListener(() =>
            {
                OnItemDeleted?.Invoke(this);
            });
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked?.Invoke(this);
        }
    }
}