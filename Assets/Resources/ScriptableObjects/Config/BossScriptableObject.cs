using Model.Card;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Config
{
    [CreateAssetMenu(fileName = "NewBoss", menuName = "ScriptableObject/Boss")]
    public class BossScriptableObject : ScriptableObject
    {
        public int Id;
        public string Title;
        public int BaseHp;
        public int BaseAttack;
        public int BaseExpReward;
        public int BaseGoldReward;
        public Sprite Image;
        public string ImagePath;
    }
}