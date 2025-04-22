using Domain.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Config
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
        public float Hp;
        public float HpRegeneration;
        public float Attack;
        public float Crit;
        public float CritDmg;
        public float Block;
        public float BlockPower;
        public float Evade;
        public Rarity Rarity;
        public Sprite Image;
        public string ImagePath;
        public bool IsStackable = true; 
        public int Count;

        public float DropChance;
    }
}