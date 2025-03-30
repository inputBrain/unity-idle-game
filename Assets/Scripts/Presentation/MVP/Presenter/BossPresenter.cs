using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class BossPresenter
    {
        private Boss _boss;

        private BossView _bossView;


        public void Init(Boss boss, BossView bossView)
        {
            _boss = boss;
            _bossView = bossView;

            UpdateSlider(_boss.MaxHp, boss.CurrentHp);

            _boss.OnCurrentHpChanged += _ => UpdateSlider();
            _boss.OnMaxHpChanged += _ => UpdateSlider();

        }

        private void UpdateSlider()
        {
            _bossView.SetSliderHp(_boss.MaxHp, _boss.CurrentHp);
        }
        
        
        private void UpdateSlider(float bossMaxHp, float bossCurrentHp)
        {
            _bossView.SetSliderHp(bossMaxHp, bossCurrentHp);
        }
    }
}