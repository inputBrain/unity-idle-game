using Steamworks;
using UnityEngine;

public class SteamInventoryExample : MonoBehaviour
{
    private SteamInventoryResult_t inventoryResult;

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam API is not initialized!");
            return;
        }

        // Запрашиваем инвентарь пользователя
        if (SteamInventory.GetAllItems(out inventoryResult))
        {
            Debug.Log("Inventory fetched successfully!");

            uint itemCount = 0;
            // Создаём массив для хранения данных о предметах
            SteamItemDetails_t[] items = new SteamItemDetails_t[100];

            // Получаем данные о предметах
            if (SteamInventory.GetResultItems(inventoryResult, items, ref itemCount))
            {
                for (int i = 0; i < itemCount; i++)
                {
                    Debug.Log($"Item {i + 1}: ID={items[i].m_iDefinition}, Quantity={items[i].m_unQuantity}");
                }
            }
            else
            {
                Debug.LogError("Failed to get items from inventory result.");
            }

            // Уничтожаем результат, чтобы освободить ресурсы
            SteamInventory.DestroyResult(inventoryResult);
        }
        else
        {
            Debug.LogError("Failed to fetch inventory.");
        }
    }
}