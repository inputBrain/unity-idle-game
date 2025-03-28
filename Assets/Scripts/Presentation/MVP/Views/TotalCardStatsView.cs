using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.MVP.Views
{
    public class TotalCardStatsView : MonoBehaviour
    {
        public TMP_Text Hp;

        public TMP_Text HPs;

        public TMP_Text Attack;

        public TMP_Text Crit;

        public TMP_Text CritDmg;

        public TMP_Text Block;

        public TMP_Text BlockPower;

        public TMP_Text Evade;
    

        [Header("Team HP Slider")]
        public Slider Slider;
        public TMP_Text HpOnSlider;



        public void SetSliderHp(float teamTotalHp, string totalHp)
        {
            Slider.value = teamTotalHp;
        
            HpOnSlider.text = totalHp;
        }


        public void SetTotalHp(float totalHp)
        {
            Hp.text = "HP: " + totalHp;
        }


        public void SetTotalHPs(float totalHpRegeneration)
        {
            HPs.text = "HP/s: " + totalHpRegeneration;
        }


        public void SetTotalAttack(float totalAttack)
        {
            Attack.text = "Attack: " + totalAttack;
        }


        public void SetTotalCrit(float totalcCrit)
        {
            Crit.text = "Crit: " + totalcCrit + "%";
        }


        public void SetTotalCritDmg(float totalCritDmg)
        {
            CritDmg.text = "Crit Dmg: " + "+" + totalCritDmg + "%";
        }


        public void SetTotalBlock(float totalBlock)
        {
            Block.text = "Block: " + totalBlock + "%";
        }


        public void SetTotalBlockPower(float totalBlockPower)
        {
            BlockPower.text = "Block Power: " + totalBlockPower + "%";
        }


        public void SetTotalEvade(float totalEvade)
        {
            Evade.text = "Evade: " + totalEvade + "%";
        }
    }
}