using System.Collections;
using Models;
using UnityEngine;

namespace UIScripts
{
    public class UIBossSubscriber : MonoBehaviour
    {
        private BossModel _boss = new();

        private UIBossController UIBossController => FindAnyObjectByType<UIBossController>();

        void Start()
        {
        
            _boss = new BossModel
            {
                MaxHp = 250,
                CurrentHP = 250f,
                Attack = 10f,
                ExpReward = 50,
                GoldReward = 1
            };
            
            InitBossUi();
        
            var delay = 1f;
            // имитация урона каждые n секунд
            StartCoroutine(Repeater(delay));
        }

        void InitBossUi()
        {
            // подписываемся на события
            _boss.OnHPChangedAction += UIBossController.SmoothSliderHpUpdate;
            _boss.OnBossDefeat += UIBossController.UpdateBossStates;

            
            
            //инициализируем значения
            UIBossController.BossHP_Slider.maxValue = _boss.MaxHp;
            UIBossController.BossHP_Slider.value = _boss.CurrentHP;

            // апдейтим ui при старте, чтобы значения были
            UIBossController.UpdateHealthText(_boss.CurrentHP, _boss.MaxHp);

            _boss.InvokeHealthChanged();
        }

        IEnumerator Repeater(float delaySec)
        {
            var damage = 10;
            while (true)
            {
                _boss.TakeDamage(damage);
                yield return new WaitForSeconds(delaySec);
            }
        }
    }
}