using System;
using System.Linq;
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

        private Transform _inventoryContainer;
        private Transform _toolbarContainer;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Transform _originalParent;
        private int _originalSiblingIndex;

        public IInventoryItem DomainItem { get; private set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        
        public void Init(EntityItem entityItem, bool isToolbar = false)
        {
            DomainItem = entityItem.BackingDomainItem;
            _inventoryContainer = UIManager.I.InventoryContainer;
            _toolbarContainer   = UIManager.I.ToolbarContainer;

            //TODO: temp set. Remove it after test
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
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            _canvasGroup.alpha = 0.7f;
            _canvasGroup.blocksRaycasts = false;
            // transform.SetParent(transform.parent.parent);
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 1) Восстановим визуал
            _canvasGroup.alpha          = 1f;
            _canvasGroup.blocksRaycasts = true;

            // 2) Попробуем свапнуть, но только если other в toolbar или в активном inventory
            var goHit = eventData.pointerCurrentRaycast.gameObject;
            var other = goHit?.GetComponentInParent<EntityView>();
            if (other != null && other != this)
            {
                var parent = other.transform.parent;
                var otherInToolbar   = parent == _toolbarContainer;
                var otherInInventory = parent == _inventoryContainer && _inventoryContainer.gameObject.activeInHierarchy;
                if (otherInToolbar || otherInInventory)
                {
                    SwapWith(other);
                    return;
                }
            }


            var pointerInToolbar   = IsPointerOver(_toolbarContainer, eventData);
            var pointerInInventory = _inventoryContainer.gameObject.activeInHierarchy && IsPointerOver(_inventoryContainer, eventData);

            Transform dropContainer = null;
    

            if (pointerInToolbar && _toolbarContainer.childCount <= 5)
            {
                dropContainer = _toolbarContainer;
            }
            else if (pointerInInventory)
            {
                dropContainer = _inventoryContainer;
            }
            
            if (dropContainer != null)
            {
                transform.SetParent(dropContainer);
                transform.SetAsLastSibling();
                OnDroppedInContainer(dropContainer == _toolbarContainer);
            }
            else
            {
                // возвращаем в исходный контейнер и ставим в конец
                transform.SetParent(_originalParent);
                transform.SetAsLastSibling();
                OnDroppedInContainer(_originalParent == _toolbarContainer);
            }
        }


        /// <summary>
        /// Проверяет, находится ли точка курсора внутри RectTransform контейнера.
        /// </summary>
        private bool IsPointerOver(Transform container, PointerEventData eventData)
        {
            if (container == null) return false;
            var rt = container as RectTransform;
            
            return RectTransformUtility.RectangleContainsScreenPoint(
                rt,
                eventData.position,
                eventData.pressEventCamera);
        }

        
        private void SwapWith(EntityView other)
        {
            var myParent   = _originalParent;
            var myIndex    = _originalSiblingIndex;
            var theirParent= other.transform.parent;
            var theirIndex = other.transform.GetSiblingIndex();


            transform.SetParent(theirParent);
            transform.SetSiblingIndex(theirIndex);

            other.transform.SetParent(myParent);
            other.transform.SetSiblingIndex(myIndex);


            OnDroppedInContainer(transform.parent == _toolbarContainer);
            other.OnDroppedInContainer(other.transform.parent == _toolbarContainer);
        }

        private void OnDroppedInContainer(bool inToolbar)
        {
            var view = GetComponent<CardView>();
            view.Slider.gameObject.SetActive(inToolbar);
            SetCountText(inToolbar ? "" : $"x{((CardModel)DomainItem).Count.Value}", !inToolbar);
        }
        
        
        
        public void OnDropped(bool inToolbar)
        {
            GetComponent<CardView>().Slider.gameObject.SetActive(inToolbar);
            // …и остальной UI-апдейт
        }
    }
}
