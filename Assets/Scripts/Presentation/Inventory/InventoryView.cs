using Presentation.Entity;
using UnityEngine;

namespace Presentation.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform itemsContainer;
        [SerializeField] private EntityView itemPrefab;

        public void ClearAllItems()
        {
            foreach (Transform t in itemsContainer) Destroy(t.gameObject);
        }

        public EntityView SpawnItemView()
            => Instantiate(itemPrefab, itemsContainer, false);
    }
}