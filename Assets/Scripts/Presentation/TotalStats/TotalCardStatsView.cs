using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.TotalStats
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

        [SerializeField]
        private float _sliderAnimDuration = 0.5f;

        public float SliderAnimDuration
        {
            get => _sliderAnimDuration;
            set => _sliderAnimDuration = value;
        }

        private Coroutine _sliderRoutine;

        public void SetSliderHp(float teamMaxHp, float teamCurrentHp, float? duration = null)
        {
            HpOnSlider.text = ((int)teamCurrentHp).ToString(CultureInfo.InvariantCulture);

            if (_sliderRoutine != null)
                StopCoroutine(_sliderRoutine);

            Slider.minValue = 0f;
            Slider.maxValue = teamMaxHp;

            var animTime = duration ?? _sliderAnimDuration;
            _sliderRoutine = StartCoroutine(AnimateSlider(teamCurrentHp, animTime));
        }

        private IEnumerator AnimateSlider(float target, float duration)
        {
            var start = Slider.value;
            var time  = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                Slider.value = Mathf.Lerp(start, target, time / duration);
                yield return null;
            }

            Slider.value = target;
        }


        public void SetTotalHp(float totalHp)
        {
            Hp.text = "HP: " + totalHp;
        }


        public void SetTotalHPRegeneration(float totalHpRegeneration)
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