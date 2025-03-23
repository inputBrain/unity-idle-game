using System.Collections;
using ScriptableObjects.Config;
using UnityEngine;

namespace OLd.Inventory
{
    public class InventoryCreator : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private Sprite lockIcon;


        private void Start()
        {
            this.StartCoroutine(CreateInventory());
        }


        IEnumerator CreateInventory()
        {
            yield return new WaitForEndOfFrame();

            foreach (var item in ItemManager.Instance.Items)
            {
                var instance = Instantiate(buttonPrefab, buttonContainer);
                instance.name = item.Name;
                var inventoryItem = instance.GetComponent<OLd.InventoryItem>();

                inventoryItem.Item = item;
                inventoryItem.Renderer.sprite = item.IsLocked ? lockIcon : item.Icon;
                inventoryItem.QuantityText.text = item.Quantity.ToString();
                inventoryItem.ConjureButton.interactable = item.CanConjure;

                inventoryItem.ConjureButton.onClick.AddListener(() => OnItemClicked(item));
            }
        }

        void OnItemClicked(Item item)
        {
            Debug.Log($"Button clicked on item: {item.Name}");
        }


        public void UpdateInventory()
        {
            foreach (Transform child in buttonContainer)
            {
                var inventoryItem = child.GetComponent<OLd.InventoryItem>();
                if (inventoryItem != null)
                {
                    var item = inventoryItem.Item;
                    inventoryItem.QuantityText.text = item.Quantity.ToString();
                    inventoryItem.Renderer.sprite = item.IsLocked ? lockIcon : item.Icon;
                }
            }
        }
    }
}