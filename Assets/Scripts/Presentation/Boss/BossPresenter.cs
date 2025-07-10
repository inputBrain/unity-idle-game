using Model.Boss;

namespace Presentation.Boss
{
    public class BossPresenter
    {
        private BossModel _bossModel;

        private BossView _bossView;


        public void Init(BossModel bossModel, BossView bossView)
        {
            _bossModel = bossModel;
            _bossView = bossView;

            UpdateSlider(_bossModel.MaxHp.Value, _bossModel.CurrentHp.Value);

            _bossModel.CurrentHp.OnValueChanged += _ => UpdateSlider();
            _bossModel.MaxHp.OnValueChanged += _ => UpdateSlider();

        }

        private void UpdateSlider()
        {
            _bossView.SetSliderHp(_bossModel.MaxHp.Value, _bossModel.CurrentHp.Value);
        }


        private void UpdateSlider(float bossMaxHp, float bossCurrentHp)
        {
            _bossView.SetSliderHp(bossMaxHp, bossCurrentHp);
        }
    }
}