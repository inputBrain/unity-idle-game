using System;
using Application.Dto;
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
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TMP_Text countText;

        public IInventoryItem DomainItem { get; private set; }

        private Action<IInventoryItem> _onSelectCallback;


        private CanvasGroup _canvasGroup;

        private RectTransform _rectTransform;

        private Transform _originalParent;

        private Vector3 _originalPosition;

        private int _originalSiblingIndex;

        
        private Transform _inventoryContainerGrid;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();

            // iconImage ДОЛЖЕН иметь raycastTarget = true для IPointerClickHandler и др
        }


        public void Init(Item item, Action<IInventoryItem> onClickCallback, Transform inventoryGrid)
        {
            DomainItem = item.BackingDomainItem;
            _onSelectCallback = onClickCallback;
            _inventoryContainerGrid = inventoryGrid;

            if (DomainItem is Domain.Entities.Card card && card.Count.Value > 1)
            {
                countText.text = $"X{card.Count.Value}";
                countText.gameObject.SetActive(true);
            }
            else
            {
                countText.text = "";
                countText.gameObject.SetActive(false);
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

            ItemView targetItemView = null;
            GameObject targetToolBarContainer = null;
            GameObject targetInventoryContainer = null;

            if (objectUnderPointer != null)
            {
                targetItemView = objectUnderPointer.GetComponentInParent<ItemView>();
                targetToolBarContainer = objectUnderPointer.CompareTag("ToolBarContainer") ? objectUnderPointer : null;
                targetInventoryContainer = objectUnderPointer.CompareTag("Inventory") ? objectUnderPointer : null;
            }

            if (targetToolBarContainer != null)
            {
                if (targetToolBarContainer.transform.childCount >= 5)
                {
                    Debug.Log("Toolbar is full!");
                    ReturnToOriginalPlace();
                    return;
                }

                // Создаём копию
                GameObject newSlot = Instantiate(gameObject, targetToolBarContainer.transform);
                var itemView = newSlot.GetComponent<ItemView>();
                itemView.SetCountText("", false); // скрыть количество
                itemView._onSelectCallback = _onSelectCallback;

                // Уменьшаем количество оригинальной карты
                if (DomainItem is Domain.Entities.Card card)
                {
                    card.Count.Value--;
                    if (card.Count.Value <= 0)
                    {
                        Destroy(gameObject); // удаляем оригинал из Inventory
                    }
                    else
                    {
                        SetCountText(card.Count.Value > 1 ? $"x{card.Count.Value}" : "", card.Count.Value > 1);
                        ReturnToOriginalPlace();
                    }
                }
            }
            else if (targetInventoryContainer != null)
            {
                // Перетаскивание обратно в инвентарь
                if (DomainItem is Domain.Entities.Card card)
                {
                    var existingItem = FindItemViewInInventory(card.Id);

                    if (existingItem != null)
                    {
                        card.Count.Value++;
                    }
                    else
                    {
                        GameObject newSlot = Instantiate(gameObject, targetInventoryContainer.transform);
                        var itemView = newSlot.GetComponent<ItemView>();
                        itemView._onSelectCallback = _onSelectCallback;
                        itemView.SetCountText("x1", true);
                    }
                }

                Destroy(gameObject);
            }
            else
            {
                ReturnToOriginalPlace();
            }
        }


        private void ReturnToOriginalPlace()
        {
            transform.SetParent(_originalParent, true);
            transform.SetSiblingIndex(_originalSiblingIndex);
            transform.localPosition = _originalPosition;
        }


        private ItemView FindItemViewInInventory(int cardId)
        {
            // Найти в контейнере Inventory существующий ItemView с этим Id
            foreach (var itemView in _inventoryContainerGrid.GetComponentsInChildren<ItemView>())
            {
                if (itemView.DomainItem.Id == cardId)
                    return itemView;
            }
            return null;
        }

        #endregion
    }
}