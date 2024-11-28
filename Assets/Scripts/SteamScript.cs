using Steamworks;
using UnityEngine;

public class SteamScript : MonoBehaviour {
    private void Start()
    {
        if (!SteamAPI.Init())
        {
            Debug.LogError("Steam API initialization failed!");
            return;
        }

        Debug.Log($"Steam API initialized successfully! User: {SteamFriends.GetPersonaName()}");
    }

    private void Update()
    {
        // Steam API должен обрабатывать коллбэки в каждом кадре
        SteamAPI.RunCallbacks();
    }

    private void OnDestroy()
    {
        // Освобождаем ресурсы Steam API при закрытии
        SteamAPI.Shutdown();
    }
}