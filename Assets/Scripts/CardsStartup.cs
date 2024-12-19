using System;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine.UI;

namespace Services
{
    public class CardsStartup : Singleton<CardsStartup>
    {
        public readonly List<CardModel> CardList = new();
        public TotalCardStatsModel TotalCardStat = new();
        
        public Slider Card1Exp_Slider;
        public Slider Card2Exp_Slider;
        public Slider Card3Exp_Slider;
        public Slider Card4Exp_Slider;
        public Slider Card5Exp_Slider;

        public TMP_Text Card1EXP_TextOnSlider;
        public TMP_Text Card2EXP_TextOnSlider;
        public TMP_Text Card3EXP_TextOnSlider;
        public TMP_Text Card4EXP_TextOnSlider;
        public TMP_Text Card5EXP_TextOnSlider;
        
        public TMP_Text Card1Level_Text;
        public TMP_Text Card2Level_Text;
        public TMP_Text Card3Level_Text;
        public TMP_Text Card4Level_Text;
        public TMP_Text Card5Level_Text;


        
        
        
        private void Start()
        {
            _initCards();
        }


        private void _initCards()
        {
            CardList.Add(new CardModel
            {
                Id = 1, 
                HP = 100,
                Title = "Epic Card",
                Level = 1, 
                ExpCurrent = 0,
                ExpToNextLevel = 500,
                StartBaseExp = 500,
                HPRegeneration = 0.1f,
                Attack = 11, 
                Crit = 0.5f,
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0,
                Evade = 0, 
                Rarity = Rarity.Rare,
                ExpSlider = Card1Exp_Slider, 
                ExpTextOnSlider = Card1EXP_TextOnSlider,
                LevelText = Card1Level_Text
            });
            
            CardList.Add(new CardModel 
            { 
                Id = 2, 
                HP = 100, 
                Title = "Legendary Card", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 1000, 
                StartBaseExp = 1000, 
                HPRegeneration = 10f, 
                Attack = 10, 
                Crit = 20, 
                CritDmg = 100, 
                Block = 25, 
                BlockPower = 50, 
                Evade = 25, 
                Rarity = Rarity.Legendary,
                ExpSlider = Card2Exp_Slider, 
                ExpTextOnSlider = Card2EXP_TextOnSlider,
                LevelText = Card2Level_Text
            });
            
            CardList.Add(new CardModel 
            { 
                Id = 3, 
                HP = 100, 
                Title = "Legendary Card 3", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 1000, 
                StartBaseExp = 1000, 
                HPRegeneration = 0, 
                Attack = 0, 
                Crit = 0, 
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0, 
                Evade = 0, 
                Rarity = Rarity.Legendary,
                ExpSlider = Card3Exp_Slider, 
                ExpTextOnSlider = Card3EXP_TextOnSlider,
                LevelText = Card3Level_Text
            });
            
            CardList.Add(new CardModel 
            { 
                Id = 4, 
                HP = 100, 
                Title = "Rare Card 4", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 100, 
                StartBaseExp = 100, 
                HPRegeneration = 0, 
                Attack = 0, 
                Crit = 0, 
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0, 
                Evade = 0, 
                Rarity = Rarity.Rare,
                ExpSlider = Card4Exp_Slider, 
                ExpTextOnSlider = Card4EXP_TextOnSlider,
                LevelText = Card4Level_Text
            });

            
            CardList.Add(new CardModel 
            { 
                Id = 5, 
                HP = 100, 
                Title = "Rare Card 5", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 100, 
                StartBaseExp = 100, 
                HPRegeneration = 0, 
                Attack = 0, 
                Crit = 0, 
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0, 
                Evade = 0, 
                Rarity = Rarity.Rare,
                ExpSlider = Card5Exp_Slider, 
                ExpTextOnSlider = Card5EXP_TextOnSlider,
                LevelText = Card5Level_Text
            });


            foreach (var card in CardList)
            {
                TotalCardStat.HP += card.HP;
                TotalCardStat.HPRegeneration += card.HPRegeneration;
                TotalCardStat.Attack += card.Attack;
                TotalCardStat.Crit += card.Crit;
                TotalCardStat.CritDmg += card.CritDmg;
                TotalCardStat.BlockChance += card.Block;
                TotalCardStat.BlockPower += card.BlockPower;
                TotalCardStat.Evade += card.Evade;
            }
        }
    }
}