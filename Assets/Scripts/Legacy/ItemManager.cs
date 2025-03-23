using System.Linq;
using ScriptableObjects.Config;
using UnityEngine;
using Utils;

namespace OLd
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;

        public Item[] Items;

        private void Awake()
        {
            Instance = this;

            //     Items = ResourceLoadUtils.GetAllScriptableObjects<Item>("");

            Items = Items.OrderBy(x => x.Id).ToArray();
            // foreach (var item in Items)
            // {
            //     Debug.Log($"Item name: {item.Name}");
            // }
        }


        public void AddItem(string id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            var updatedItems = Items[Items.Length + 1];
        }
    }
}
