namespace Presentation.Boss
{
    public class BossPresenter
    {
        private Domain.Entities.Boss _boss;

        private BossView _bossView;


        public void Init(Domain.Entities.Boss boss, BossView bossView)
        {
            _boss = boss;
            _bossView = bossView;

            UpdateSlider(_boss.MaxHp.Value, boss.CurrentHp.Value);

            _boss.CurrentHp.OnValueChanged += _ => UpdateSlider();
            _boss.MaxHp.OnValueChanged += _ => UpdateSlider();

        }

        private void UpdateSlider()
        {
            _bossView.SetSliderHp(_boss.MaxHp.Value, _boss.CurrentHp.Value);
        }


        private void UpdateSlider(float bossMaxHp, float bossCurrentHp)
        {
            _bossView.SetSliderHp(bossMaxHp, bossCurrentHp);
        }
    }
}