using System;
using Api.Payload;
using Model.Card;
using Model.InventoryCard;
using Presentation.Card;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Presentation.Entity
{
    public class EntityView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text countText;

        [SerializeField] private Transform InventoryContainer;
        [SerializeField] private Transform ToolbarContainer;

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

        public void Init(EntityItem entityItem, Action<IInventoryItem> onClickCallback, Transform inventoryGrid, bool isToolbar = false)
        {
            DomainItem = entityItem.BackingDomainItem;
            _onSelectCallback = onClickCallback;
            _inventoryContainerGrid = inventoryGrid;

            if (DomainItem is CardModel card)
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
                iconImage.sprite = entityItem.DisplayIcon;
                iconImage.enabled = entityItem.DisplayIcon != null;
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
            transform.SetParent(transform.parent.parent);
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

            var targetItemView = targetObject.GetComponentInParent<EntityView>();
            var targetContainer = targetObject.GetComponentInParent<Transform>();

            //Если под нами карточка и єто не мі
            if (targetItemView != null)
            {
                Transform swappedParent = targetItemView.transform.parent;
                
                bool inToolbar = swappedParent.gameObject.CompareTag("ToolBarContainer");
                bool inInventory = swappedParent.gameObject.CompareTag("InventoryContainer");
                
                if (inToolbar || inInventory)
                {
                    int swappedSiblingIndex = targetItemView.transform.GetSiblingIndex();

                    this.transform.SetParent(swappedParent);
                    this.transform.SetSiblingIndex(swappedSiblingIndex);

                    targetItemView.transform.SetParent(_originalParent);
                    targetItemView.transform.SetSiblingIndex(_originalSiblingIndex);

                    if (inToolbar)
                    {
                        GetComponent<CardView>()?.Slider.gameObject.SetActive(true);
                        SetCountText($"x{((CardModel)DomainItem).Count.Value}", false);
                        
                        targetItemView.GetComponent<CardView>()?.Slider.gameObject.SetActive(false);
                        targetItemView.SetCountText($"x{((CardModel)DomainItem).Count.Value}", true);
                    }
                    else
                    {
                        GetComponent<CardView>()?.Slider.gameObject.SetActive(false);
                        SetCountText($"x{((CardModel)DomainItem).Count.Value}", true);
                        
                        targetItemView.GetComponent<CardView>()?.Slider.gameObject.SetActive(false);
                        targetItemView.SetCountText($"x{((CardModel)DomainItem).Count.Value}", true);
                    }
                }

                return;
            }

            //TODO: 
            if (targetContainer != null && targetContainer.childCount < 5)
            {
                transform.SetParent(targetContainer);
                transform.localPosition = Vector3.zero;

                bool inToolbar = targetContainer.CompareTag("ToolBarContainer");
                GetComponent<CardView>()?.Slider.gameObject.SetActive(inToolbar);
                SetCountText(inToolbar ? "" : $"x{((CardModel)DomainItem).Count.Value}", !inToolbar);
                return;
            }

            ReturnToOriginalPlace();
        }

        private void ReturnToOriginalPlace()
        {
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(_originalSiblingIndex);
        }
    }
}
