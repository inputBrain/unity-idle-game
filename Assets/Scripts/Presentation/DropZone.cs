using Presentation.Entity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Presentation
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] bool isToolbarZone;
        public void OnDrop(PointerEventData e)
        {
            var card = e.pointerDrag?.GetComponent<EntityView>();
            if (card==null) return;
            card.transform.SetParent(transform);
            card.OnDroppedInContainer(isToolbarZone);
        }
    }

}