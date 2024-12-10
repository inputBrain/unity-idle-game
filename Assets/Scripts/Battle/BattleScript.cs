using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Battle
{
    public class BattleScript : MonoBehaviour
    {
        public List<CardModel> CardList = new();

        public TMP_Text Zone_Text;
        public int Zone = 1;
        
        public CardModel[] Cards;
        public TotalCardStatsModel TotalCardStat;

        public BossModel Boss;
        
        public Slider BossArmor_Slider;
        public TMP_Text BossArmor_TextOnSlider;
        
        public Slider BossHP_Slider;
        public TMP_Text BossHP_TextOnSlider;

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
        
        public Image CardImage1;
        public Image CardImage2;
        public Image CardImage3;
        public Image CardImage4;
        public Image CardImage5;

        

        private void Start()
        {
            Boss = new BossModel()
            {
                Id = 1,
                Armor = 50f,
                Attack = 5f,
                HP = 500f,
                Image = new Image(),
                Title = "Strong Boss"
            };
            
            
            CardList.Add(new CardModel() {
                    Id = 2,
                    Title = "Second Card",
                    Level = 1,
                    ExpCurrent = 0f,
                    ExpToNextLevel = 100f,
                    HP = 10,
                    HPRegeneration = 0.1f,
                    Attack = 1f,
                    Crit = 1f,
                    CritDmg = 25f,
                    Block = 0.1f,
                    BlockPower = 25f,
                    Evade = 1f,
                    Rarity = Rarity.Rare,
                    Image = CardImage2
                }
            );            
            
            CardList.Add(new CardModel() {
                    Id = 3,
                    Title = "Third Card",
                    Level = 1,
                    ExpCurrent = 0f,
                    ExpToNextLevel = 100f,
                    HP = 10,
                    HPRegeneration = 0.1f,
                    Attack = 1f,
                    Crit = 1f,
                    CritDmg = 25f,
                    Block = 0.1f,
                    BlockPower = 25f,
                    Evade = 1f,
                    Rarity = Rarity.Epic,
                    Image = CardImage3
                }
            );            
            
            CardList.Add(new CardModel() {
                    Id = 4,
                    Title = "Four Card",
                    Level = 1,
                    ExpCurrent = 0f,
                    ExpToNextLevel = 100f,
                    HP = 10,
                    HPRegeneration = 0.1f,
                    Attack = 1f,
                    Crit = 1f,
                    CritDmg = 25f,
                    Block = 0.1f,
                    BlockPower = 25f,
                    Evade = 1f,
                    Rarity = Rarity.Epic,
                    Image = CardImage4
                }
            );            
            
            CardList.Add(new CardModel() {
                    Id = 5,
                    Title = "Five Card",
                    Level = 1,
                    ExpCurrent = 0f,
                    ExpToNextLevel = 100f,
                    HP = 10,
                    HPRegeneration = 0.1f,
                    Attack = 1f,
                    Crit = 1f,
                    CritDmg = 25f,
                    Block = 0.1f,
                    BlockPower = 25f,
                    Evade = 1f,
                    Rarity = Rarity.Epic,
                    Image = CardImage5
                }
            );            
            
            CardList.Add(new CardModel() {
                    Id = 1,
                    Title = "First Card",
                    Level = 1,
                    ExpCurrent = 0f,
                    ExpToNextLevel = 100f,
                    HP = 10,
                    HPRegeneration = 0.1f,
                    Attack = 1f,
                    Crit = 1f,
                    CritDmg = 25f,
                    Block = 0.1f,
                    BlockPower = 25f,
                    Evade = 1f,
                    Rarity = Rarity.Epic,
                    Image = CardImage1
                }
            );

            
            Zone_Text.text = "Zone: " + Zone;
            
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


            if (Boss.Armor > 0.1f)
            {
                BossArmor_Slider.value = 1f;
                BossArmor_TextOnSlider.text = Boss.Armor.ToString("F1");
            }
            
            TeamHP_TextOnSlider.text = TotalCardStat.HP.ToString();
            TeamHP_Slider.value = 1f;
        }



        private async void Update()
        {
            if (Boss.Armor >= 0.1f)
            {
                Boss.Armor -= TotalCardStat.Attack;
                BossArmor_TextOnSlider.text = Boss.Armor.ToString("F1");
            }
            else
            {
                Boss.HP -= TotalCardStat.Attack;
                BossHP_TextOnSlider.text = Boss.HP.ToString("F1");
                await Task.Delay(1_000);
            }
        }
        
    }
    
}