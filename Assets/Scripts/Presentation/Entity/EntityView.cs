using Model.Card;
using Model.InventoryCard;
using Presentation.Card;
using Presentation.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Presentation.Entity
{
    public class EntityView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private CardModel _cardModel;
        private InventoryPresenter _inventoryPresenter;
        
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
            _canvasGroup   = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();

            _inventoryContainer = UIManager.I.InventoryContainer;
            _toolbarContainer   = UIManager.I.ToolbarContainer;
        }


        public void Init(CardModel cardModel, InventoryPresenter inventoryPresenter, bool isToolbar = false)
        {
            _cardModel          = cardModel;
            _inventoryPresenter = inventoryPresenter;
            DomainItem          = cardModel;

            if (iconImage != null && cardModel.IconResourcesPath.Value != null)
                iconImage.sprite = Resources.Load<Sprite>(cardModel.IconResourcesPath.Value);


            var cardView = GetComponent<CardView>();
            if (cardView != null)
                cardView.Slider.gameObject.SetActive(isToolbar);
        }
        
        

        // public void SetCountText(string text, bool visible)
        // {
        //     countText.text = text;
        //     countText.gameObject.SetActive(visible);
        // }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = false;
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha          = 1f;
            _canvasGroup.blocksRaycasts = true;

            var other = eventData.pointerCurrentRaycast.gameObject?.GetComponentInParent<EntityView>();
            if (other != null && other != this)
            {
                var parent = other.transform.parent;
                var isOtherToolbar   = parent == _toolbarContainer;
                var isOtherInventory = parent == _inventoryContainer && _inventoryContainer.gameObject.activeInHierarchy;
                if (isOtherToolbar || isOtherInventory)
                {
                    SwapWith(other);
                    return;
                }
            }

            var pointerInToolbar   = IsPointerOver(_toolbarContainer, eventData);
            var pointerInInventory = _inventoryContainer.gameObject.activeInHierarchy && IsPointerOver(_inventoryContainer, eventData);

            Transform dropContainer = null;
            var isToolbarZone = false;

            if (pointerInToolbar && _toolbarContainer.childCount < 6)
            {
                dropContainer = _toolbarContainer;
                isToolbarZone = true;
            }
            else if (pointerInInventory)
            {
                dropContainer = _inventoryContainer;
                isToolbarZone  = false;
            }

            if (dropContainer != null)
            {
                if (!isToolbarZone && _originalParent == _toolbarContainer && _toolbarContainer.childCount == 1)
                {
                    transform.SetParent(_toolbarContainer);
                    transform.SetAsLastSibling();
                    OnDroppedInContainer(true);
                    return;
                }

                transform.SetParent(dropContainer);
                transform.SetAsLastSibling();


                if (dropContainer != _originalParent)
                {
                    if (isToolbarZone && !_inventoryPresenter.IsSelected(_cardModel))
                        _inventoryPresenter.ToggleSelection(_cardModel);
                    else if (!isToolbarZone && _inventoryPresenter.IsSelected(_cardModel))
                        _inventoryPresenter.ToggleSelection(_cardModel);
                }

                OnDroppedInContainer(isToolbarZone);
            }
            else
            {
                transform.SetParent(_originalParent);
                transform.SetSiblingIndex(_originalSiblingIndex);
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

        public void OnDroppedInContainer(bool isToolbarZone)
        {
            var view = GetComponent<CardView>();

            if (isToolbarZone)
            {
                view.Slider.gameObject.SetActive(true);
                view.Level.gameObject.SetActive(true);
                view.Rank.gameObject.SetActive(false);
                view.CountText.gameObject.SetActive(false);
                view.ExpCurrent.gameObject.SetActive(true);
                view.ExpToNextLevel.gameObject.SetActive(true);

                view.SetLevel(_cardModel.Level.Value);
                view.SetSliderNextExp(_cardModel.ExpToNextLevel.Value);
                view.SetSliderCurrentExp(_cardModel.ExpCurrent.Value);
                view.SetTextNextExp(_cardModel.ExpToNextLevel.Value);
                view.SetTextCurrentExp(_cardModel.ExpCurrent.Value);
            }
            else
            {
                view.Slider.gameObject.SetActive(false);
                view.Level.gameObject.SetActive(false);
                view.Rank.gameObject.SetActive(true);
                view.ExpCurrent.gameObject.SetActive(false);
                view.ExpToNextLevel.gameObject.SetActive(false);

                view.CountText.gameObject.SetActive(true);
                view.SetCount(_cardModel.Count.Value);
                view.SetRank(_cardModel.Rank.Value);
            }
        }
    }
}
