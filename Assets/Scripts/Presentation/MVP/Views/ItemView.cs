using System;
using Application.Dto;
using Domain.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Presentation.MVP.Views
{
    public class ItemView : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image selectionHighlight;
        [SerializeField] private Button deleteButton;

        public IInventoryItem DomainItem { get; private set; }

        private Action<IInventoryItem> _onClickCallback;
        private Action<IInventoryItem> _onDeleteCallback;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Transform _originalParent; //Изначальный родитель
        private Vector3 _originalPosition;
        private int _originalSiblingIndex; // Исходный индекс в иерархии родителя

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();

            if (deleteButton != null)
                deleteButton.onClick.AddListener(HandleDeleteClick);

            if (selectionHighlight != null)
                selectionHighlight.gameObject.SetActive(false);

            // iconImage ДОЛЖЕН иметь raycastTarget = true для IPointerClickHandler и др
        }

        public void Init(Item item, Action<IInventoryItem> onClickCallback, Action<IInventoryItem> onDeleteCallback)
        {
            DomainItem = item.BackingDomainItem;
            _onClickCallback = onClickCallback;
            _onDeleteCallback = onDeleteCallback;

            if (iconImage != null)
            {
                iconImage.sprite = item.DisplayIcon;
                iconImage.enabled = item.DisplayIcon != null;
            }

            SetSelected(false);
        }

        public void SetSelected(bool isSelected)
        {
            if (selectionHighlight != null)
                selectionHighlight.gameObject.SetActive(isSelected);
        }

        private void HandleDeleteClick()
        {
            _onDeleteCallback?.Invoke(DomainItem);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == deleteButton?.gameObject) return;
            _onClickCallback?.Invoke(DomainItem);
        }

        private void OnDestroy()
        {
            if (deleteButton != null)
                deleteButton.onClick.RemoveListener(HandleDeleteClick);
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
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            GameObject objectUnderPointer = eventData.pointerEnter; 
            ItemView targetItemView = null;

            if (objectUnderPointer != null)
            {
                targetItemView = objectUnderPointer.GetComponentInParent<ItemView>();
            }

            bool canSwap = targetItemView != null &&             // Нашли ItemView?
                           targetItemView != this &&               //Это не мы сами?
                           targetItemView.transform.parent == _originalParent; // Он в том же родителе?

            if (canSwap)
            {
                transform.SetSiblingIndex(targetItemView.transform.GetSiblingIndex());

                targetItemView.transform.SetParent(_originalParent, true);
                targetItemView.transform.SetSiblingIndex(_originalSiblingIndex);
                
                 _rectTransform.localPosition = Vector3.zero;
                 targetItemView._rectTransform.localPosition = Vector3.zero;
                 
                Debug.Log($"Swapped {this.DomainItem?.Title} with {targetItemView.DomainItem?.Title}");
            
            }
            else
            {
                transform.SetSiblingIndex(_originalSiblingIndex); // Возвращаем на исходный индекс
                transform.localPosition = _originalPosition;
                Debug.Log($"Returned {this.DomainItem?.Title} to original position.");
            }
        }

        #endregion

    }
}