using Steamworks;
using UnityEngine;

public class SteamAchievement : MonoBehaviour
{
    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam API не инициализирована!");
            return;
        }

        // Название достижения из Steamworks Dashboard (пример: "ACH_WIN_ONE_GAME")
        string achievementName = "ACH_WIN_ONE_GAME";

        // Проверяем, есть ли достижение
        bool isAchieved;
        SteamUserStats.GetAchievement(achievementName, out isAchieved);
        Debug.Log($"Достижение {achievementName} разблокировано: {isAchieved}");

        // Если достижение ещё не разблокировано, разблокируем его
        if (!isAchieved)
        {
            SteamUserStats.SetAchievement(achievementName);
            SteamUserStats.StoreStats(); // Сохраняем изменения на сервере
            Debug.Log($"Достижение {achievementName} разблокировано!");
        }
    }
}