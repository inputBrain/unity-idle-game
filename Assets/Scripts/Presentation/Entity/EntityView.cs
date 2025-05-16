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
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha         = 1f;
            _canvasGroup.blocksRaycasts = true;

            // 1) Сначала пробуем свапнуть
            var other = eventData.pointerCurrentRaycast.gameObject?.GetComponentInParent<EntityView>();
            if (other != null && other != this)
            {
                SwapWith(other);
                return;
            }

            // 2) Ищем контейнер в hovered-списке
            Transform dropContainer = eventData.hovered
                .Select(go => GetDropContainer(go))
                .FirstOrDefault(c => c != null);

            // 3) Fallback: проверяем попадание курсора внутрь Rect, даже без Graphic Raycast Target
            if (dropContainer == null)
            {
                if (IsPointerInRect(_inventoryContainer, eventData))
                    dropContainer = _inventoryContainer;
                else if (IsPointerInRect(_toolbarContainer, eventData))
                    dropContainer = _toolbarContainer;
            }

            // 4) Если контейнер найден — вставляем в конец
            if (dropContainer != null)
            {
                transform.SetParent(dropContainer);
                transform.SetSiblingIndex(dropContainer.childCount - 1);
                OnDroppedInContainer(dropContainer == _toolbarContainer);
            }
            else
            {
                // за пределами зон — возвращаем на старое место
                ReturnToOriginalPlace();
            }
        }



        private void ReturnToOriginalPlace()
        {
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(_originalSiblingIndex);
            OnDroppedInContainer(_originalParent == _toolbarContainer);
        }
        
        
        private bool IsPointerInRect(Transform container, PointerEventData eventData)
        {
            var rt = container as RectTransform;
            return RectTransformUtility.RectangleContainsScreenPoint(
                rt,
                eventData.position,
                eventData.pressEventCamera);
        }

        private Transform GetDropContainer(GameObject go)
        {
            if (go == null) return null;
            if (go.transform == _inventoryContainer || go.transform.IsChildOf(_inventoryContainer))
                return _inventoryContainer;
            if (go.transform == _toolbarContainer   || go.transform.IsChildOf(_toolbarContainer))
                return _toolbarContainer;
            return null;
        }


        
        private void SwapWith(EntityView other)
        {
            var myParent   = _originalParent;
            var myIndex    = _originalSiblingIndex;
            var theirParent= other.transform.parent;
            var theirIndex = other.transform.GetSiblingIndex();

            // меняем местами
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
