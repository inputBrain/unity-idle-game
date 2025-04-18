using System;
using Application.Dto;
using Domain.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Presentation.MVP.Views
{
    public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image iconImage;
        public IInventoryItem DomainItem { get; private set; }

        private Action<IInventoryItem> _onSelectCallback;


        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Transform _originalParent; //Изначальный родитель
        private Vector3 _originalPosition;
        private int _originalSiblingIndex; // Исходный индекс в иерархии родителя

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();

            // iconImage ДОЛЖЕН иметь raycastTarget = true для IPointerClickHandler и др
        }

        public void Init(Item item, Action<IInventoryItem> onClickCallback)
        {
            DomainItem = item.BackingDomainItem;
            _onSelectCallback = onClickCallback;
       

            if (iconImage != null)
            {
                iconImage.sprite = item.DisplayIcon;
                iconImage.enabled = item.DisplayIcon != null;
            }
        }
        

        #region Drag and Drop

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = transform.localPosition;
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            _canvasGroup.alpha = 0.7f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            GameObject objectUnderPointer = eventData.pointerEnter; 
            
            
            //
            ItemView targetItemView = null;
            GameObject targetToolBarContainer = null;
            GameObject targetInventoryContainer = null;
            if (objectUnderPointer != null)
            {
                targetItemView = objectUnderPointer.GetComponentInParent<ItemView>();
                targetToolBarContainer = objectUnderPointer.gameObject.CompareTag($"ToolBarContainer") ? objectUnderPointer.gameObject : null;
                targetInventoryContainer = objectUnderPointer.gameObject.CompareTag($"Inventory") ? objectUnderPointer.gameObject : null;
            }

            bool canSwap = targetItemView != null && // Нашли ItemView?
                           targetItemView != this;         //Это не мы сами?
                           // targetItemView.transform.parent == _originalParent; // Он в том же родителе?

            if (canSwap)
            {
                transform.SetSiblingIndex(targetItemView.transform.GetSiblingIndex());

                targetItemView.transform.SetParent(_originalParent, true);
                targetItemView.transform.SetSiblingIndex(_originalSiblingIndex);

                _rectTransform.localPosition = Vector3.zero;
                targetItemView._rectTransform.localPosition = Vector3.zero;

                Debug.Log($"Swapped {this.DomainItem?.Title} with {targetItemView.DomainItem?.Title}");
                
                
                // Inventory ToolBar
                
                //Inventory - A - Select
                //ToolBar - B (Select for game)
                
                //Inventory - _onSelectCallback( B.GetComp<View>.DomainItem)
                //ToolBar - A - Select
                
            }
            else
            {
                transform.SetSiblingIndex(_originalSiblingIndex); // Возвращаем на исходный индекс
                transform.localPosition = _originalPosition;
                Debug.Log($"Returned {this.DomainItem?.Title} to original position.");
            }


            if (targetToolBarContainer != null) //Нашли тулбар
            {
                //вызываем событие _onSelectCallback
                _onSelectCallback?.Invoke(DomainItem);
                transform.SetParent(targetToolBarContainer.transform, true);
                var go = gameObject.GetComponent<CardView>();
                go.Slider.gameObject.SetActive(true);
                // _onSelectCallback?.Invoke(DomainItem);
            }


            if (targetInventoryContainer != null)
            {
                _onSelectCallback?.Invoke(DomainItem);
                transform.SetParent(targetInventoryContainer.transform, true);
                // Сетим родителя
                var go = gameObject.GetComponent<CardView>();
                go.Slider.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}