using System;
using Application.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Presentation.Card;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Presentation.Inventory
{
    public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text countText;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Transform _originalParent;
        private Vector3 _originalPosition;
        private int _originalSiblingIndex;
        private Transform _inventoryContainerGrid;

        public IInventoryItem DomainItem { get; private set; }
        private Action<IInventoryItem> _onSelectCallback;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Item item, Action<IInventoryItem> onClickCallback, Transform inventoryGrid, bool isToolbar = false)
        {
            DomainItem = item.BackingDomainItem;
            _onSelectCallback = onClickCallback;
            _inventoryContainerGrid = inventoryGrid;

            if (DomainItem is Domain.Entities.Card card)
            {
                SetCountText(isToolbar ? "" : (card.Count.Value > 1 ? $"x{card.Count.Value}" : ""), !isToolbar && card.Count.Value > 1);

                var cardView = GetComponent<CardView>();
                if (cardView != null)
                {
                    cardView.Slider.gameObject.SetActive(isToolbar);
                }
            }

            if (iconImage != null)
            {
                iconImage.sprite = item.DisplayIcon;
                iconImage.enabled = item.DisplayIcon != null;
            }
        }

        public void SetCountText(string text, bool visible)
        {
            countText.text = text;
            countText.gameObject.SetActive(visible);
        }

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

            GameObject targetObject = eventData.pointerEnter;
            if (targetObject == null)
            {
                ReturnToOriginalPlace();
                return;
            }

            var targetItemView = targetObject.GetComponentInParent<ItemView>();
            var targetContainer = targetObject.GetComponentInParent<Transform>();

            if (targetItemView != null && targetItemView != this)
            {
                Transform parentA = this.transform.parent;
                int indexA = this.transform.GetSiblingIndex();

                Transform parentB = targetItemView.transform.parent;
                int indexB = targetItemView.transform.GetSiblingIndex();

                this.transform.SetParent(parentB);
                this.transform.SetSiblingIndex(indexB);

                targetItemView.transform.SetParent(parentA);
                targetItemView.transform.SetSiblingIndex(indexA);

                _rectTransform.localPosition = Vector3.zero;
                targetItemView._rectTransform.localPosition = Vector3.zero;

                bool inToolbar = parentB.CompareTag("ToolBarContainer");
                GetComponent<CardView>()?.Slider.gameObject.SetActive(inToolbar);
                SetCountText(inToolbar ? "" : $"x{((Domain.Entities.Card)DomainItem).Count.Value}", !inToolbar);

                inToolbar = parentA.CompareTag("ToolBarContainer");
                targetItemView.GetComponent<CardView>()?.Slider.gameObject.SetActive(inToolbar);
                targetItemView.SetCountText(inToolbar ? "" : $"x{((Domain.Entities.Card)targetItemView.DomainItem).Count.Value}", !inToolbar);

                return;
            }

            if (targetContainer != null && targetContainer.childCount < 5)
            {
                transform.SetParent(targetContainer);
                transform.localPosition = Vector3.zero;

                bool inToolbar = targetContainer.CompareTag("ToolBarContainer");
                GetComponent<CardView>()?.Slider.gameObject.SetActive(inToolbar);
                SetCountText(inToolbar ? "" : $"x{((Domain.Entities.Card)DomainItem).Count.Value}", !inToolbar);
                return;
            }

            ReturnToOriginalPlace();
        }

        private void ReturnToOriginalPlace()
        {
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(_originalSiblingIndex);
            transform.localPosition = Vector3.zero;
        }
    }
}
