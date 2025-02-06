using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "ScriptableObject/Card")]
    public class CardObject : ScriptableObject
    {
        public int Id;
        public string Title;
        public int Level = 1;
        public float ExpCurrent;
        public float ExpToNextLevel;
        public float StartBaseExp;
        public float HP;
        public float HPRegeneration;
        public float Attack;
        public float Crit;
        public float CritDmg;
        public float Block;
        public float BlockPower;
        public float Evade;
        public RarityObject Rarity;
        public Sprite Image;
        public bool IsStackable = true; 
    }
    
    public enum RarityObject
    {
        Common = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4
    }
}