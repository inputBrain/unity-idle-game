using TMPro;
using UnityEngine;

namespace Presentation.MVP.Views
{
    public class TotalCardStatsView : MonoBehaviour
    {
        [Header("Хпшка")]
        public TMP_Text Hp;

        public TMP_Text HPs;

        public TMP_Text Attack;

        public TMP_Text Crit;

        public TMP_Text CritDmg;

        public TMP_Text Block;

        public TMP_Text BlockPower;

        public TMP_Text Evade;
        
        
        public void SetHp(float hp)
        {
            Hp.text = "HP: " + hp;
        }
    }
}