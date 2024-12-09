using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Battle
{
    public class BattleScript : MonoBehaviour
    {
        public CardModel[] Cards;
        public TotalCardStatsModel TotalCardStat;

        public BossModel Boss;
        public Slider BossArmor_Slider;
        public Slider BossHP_TextSlider;
        
        public Slider TeamHP_Slider;
        public TMP_Text TeamHP_TextOnSlider;
        
        public TMP_Text HP_Text;
        public TMP_Text HPs_Text;
        public TMP_Text Attack_Text;
        public TMP_Text Crit_Text;
        public TMP_Text CritDmg_Text;
        public TMP_Text Block_Text;
        public TMP_Text BlockPower_Text;
        public TMP_Text Evade_Text;

        

        private void Start()
        {
            foreach (var card in Cards)
            {
                TotalCardStat.ExpCurrent += card.ExpCurrent;
                TotalCardStat.ExpToNextLevel += card.ExpToNextLevel;
                TotalCardStat.HP += card.HP;
                TotalCardStat.HPRegeneration += card.HPRegeneration;
                TotalCardStat.Attack += card.Attack;
                TotalCardStat.Crit += card.Crit;
                TotalCardStat.Block += card.Block;
                TotalCardStat.BlockPower += card.BlockPower;
                TotalCardStat.Evade += card.Evade;
            }

            HP_Text.text = "HP:" + TotalCardStat.HP;
            HPs_Text.text = "HP/s:" + TotalCardStat.HPRegeneration;
            Attack_Text.text = "Attack:" + TotalCardStat.Attack;
            Crit_Text.text = "Crit:" + TotalCardStat.Crit;
            CritDmg_Text.text = "Crit Dmg:" + TotalCardStat.CritDmg;
            Block_Text.text = "Block:" + TotalCardStat.Block;
            BlockPower_Text.text = "Block Power:" + TotalCardStat.BlockPower;
            Evade_Text.text = "Evade:" + TotalCardStat.Evade;
        }



        private void Update()
        {
            throw new NotImplementedException();
        }
    }
}