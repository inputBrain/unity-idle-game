using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class UIBossController : Singleton<UIBossController>
    {
        public Slider BossHP_Slider;

        public TMP_Text BossHP_TextOnSlider;
        public TMP_Text BossAttack_Text;
        public TMP_Text BossExpReward_Text;
        public TMP_Text BossGoldReward_Text;
        
        public float lerpDuration = 0.5f;
        
        private Coroutine _currentCoroutine;
    
    
        public void SmoothSliderHpUpdate(float currentHp, float maxHp)
        {
            var startValue = BossHP_Slider.value;
            BossHP_Slider.maxValue = maxHp;
        
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
        
            _currentCoroutine = StartCoroutine(LerpSlider(startValue, currentHp, currentHp, maxHp));
        }


        public void UpdateBossStates(float attack, float exp, float gold)
        {
            BossAttack_Text.text = "Attack: " + attack.ToString("F1");
            BossExpReward_Text.text = "Exp: " + exp.ToString("F1");
            BossGoldReward_Text.text = "Gold: " + gold.ToString("F1");
        }
    
        IEnumerator LerpSlider(float startValue, float targetValue, float currentHp, float maxHp)
        {
            var timeElapsed = 0f;
            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                var newValue = Mathf.Lerp(startValue, targetValue, timeElapsed / lerpDuration);
                BossHP_Slider.value = newValue;

                var displayedHp = Mathf.RoundToInt(newValue);
                UpdateHealthText(displayedHp, maxHp);

                yield return null;
            }
            BossHP_Slider.value = targetValue;
            UpdateHealthText(currentHp, maxHp);
        }
    
        public void UpdateHealthText(float currentHp, float maxHp)
        {
            BossHP_TextOnSlider.text = $"{Mathf.RoundToInt(currentHp)}/{Mathf.RoundToInt(maxHp)}";
        }
        
    }
}