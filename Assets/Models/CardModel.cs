﻿using TMPro;
using UnityEngine.UI;

namespace Models
{
    public class CardModel
    {
        public int Id;
        public string Title;
        public int Level;
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
        public Rarity Rarity;
        public Image Image;
        public Slider ExpSlider;
        public TMP_Text ExpTextOnSlider;
        public TMP_Text LevelText;
    }
}