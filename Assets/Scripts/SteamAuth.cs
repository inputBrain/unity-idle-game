using Steamworks;
using UnityEngine;

public class SteamAuth : MonoBehaviour
{
    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam API is not init!");
            return;
        }

        var personaName = SteamFriends.GetPersonaName();
        Debug.Log($"Steam user: {personaName}");
        
        var steamID = SteamUser.GetSteamID();
        Debug.Log($"Steam ID: {steamID}");
    }
}