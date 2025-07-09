using Model.Boss;
using Model.Card;

namespace Presentation.BossStats
{
    public class BossStatsPresenter
    {
        private BossModel _bossModel;
        private BossStatsView _bossStatsView;

        public void Init(BossModel bossModel, BossStatsView bossStatsView)
        {
            _bossModel = bossModel;
            _bossStatsView = bossStatsView;
            
            UpdateIcon(_bossModel.IconResourcesPath.Value);
            _bossStatsView.SetAttack(_bossModel.Attack.Value);
            _bossStatsView.SetExperienceReward(_bossModel.ExpReward.Value);
            _bossStatsView.SetGoldReward(_bossModel.GoldReward.Value);

            _bossModel.IconResourcesPath.OnValueChanged += UpdateIcon;

            _bossModel.Attack.OnValueChanged += _bossStatsView.SetAttack;
            _bossModel.ExpReward.OnValueChanged += _bossStatsView.SetExperienceReward;
            _bossModel.GoldReward.OnValueChanged += _bossStatsView.SetGoldReward;

        }


        private void UpdateIcon(string path)
        {
            _bossStatsView.SetImage(path);
        }


        private void UpdateAttack(float value)
        {
            _bossStatsView.SetAttack(value);
        }

        private void UpdateExperienceReward(int value)
        {
            _bossStatsView.SetExperienceReward(value);
        }

        private void UpdateGoldReward(int value)
        {
            _bossStatsView.SetGoldReward(value);
        }
    }
}