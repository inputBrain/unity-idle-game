
using System.Collections;
using UnityEngine;

public class InventoryCreator : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Sprite lockIcon;


    private void Start()
    {
        StartCoroutine(CreateInventory());
    }


    IEnumerator CreateInventory()
    {
        yield return new WaitForEndOfFrame();

        foreach (var item in ItemManager.Instance.Items)
        {
            var instance = Instantiate(buttonPrefab, buttonContainer);
            instance.name = item.name;
            var inventoryItem = instance.GetComponent<InventoryItem>();

            inventoryItem.Item = item;
            inventoryItem.Renderer.sprite = item.Icon;
        }
    }
}