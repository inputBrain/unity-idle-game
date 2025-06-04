using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Boss
{
    public class BossView : MonoBehaviour
    {
        [Header("Boss HP Slider")]
        public Slider Slider;
        public TMP_Text HpOnSlider;


        [SerializeField]
        private float _sliderAnimDuration = 0.5f;

        private Coroutine _sliderRoutine;

        public void SetSliderHp(float bossMaxHp, float bossCurrentHp)
        {
            var target = bossCurrentHp / bossMaxHp;

            HpOnSlider.text = ((int)bossCurrentHp).ToString(CultureInfo.InvariantCulture);

            if (_sliderRoutine != null)
                StopCoroutine(_sliderRoutine);

            _sliderRoutine = StartCoroutine(AnimateSlider(target));
        }

        private IEnumerator AnimateSlider(float target)
        {
            var start = Slider.value;
            var time = 0f;

            while (time < _sliderAnimDuration)
            {
                time += Time.deltaTime;
                Slider.value = Mathf.Lerp(start, target, time / _sliderAnimDuration);
                yield return null;
            }

            Slider.value = target;
        }
    }
}