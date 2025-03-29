using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.MVP.Views
{
    public class BossView : MonoBehaviour
    {
        [Header("Boss HP Slider")]
        public Slider Slider;
        public TMP_Text HpOnSlider;


        public void SetSliderHp(float bossMaxHp, float bossCurrentHp)
        {
            Slider.value = bossCurrentHp / bossMaxHp;

            HpOnSlider.text = bossMaxHp.ToString(CultureInfo.InvariantCulture);
        }
    }
}