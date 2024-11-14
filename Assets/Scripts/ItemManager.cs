using ScriptableObjects;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public Item[] Items;

    private void Awake()
    {
        Instance = this;

        Items = Utils.GetAllInstances<Item>();
    }
}
