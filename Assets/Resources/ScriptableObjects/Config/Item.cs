using UnityEngine;

namespace ScriptableObjects.Config
{
    [CreateAssetMenu(fileName = "NewsItem", menuName = "ScriptableObject/ConfigItem")]
    public class Item : ScriptableObject
    {
        public string Id;
        
        public string Name;

        public bool CanConjure;

        public Sprite Icon;

        public bool IsLocked;

        public int Quantity;

        public int PerSecond;
    }
}