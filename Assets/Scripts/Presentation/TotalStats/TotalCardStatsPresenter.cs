using System.Collections.Generic;
using System.Linq;
using Model.Card;

namespace Presentation.TotalStats
{
    public class TotalCardStatsPresenter
    {
        private TotalCardStatsView _view;
        private List<CardModel>    _cards;
        private readonly Dictionary<CardModel, float> _lastHp = new();
        
        public void Init(List<CardModel> cards, TotalCardStatsView view)
        {
            _cards = cards;
            _view = view;
            
            InitTotalStats();
    
            foreach (var card in _cards)
            {
                _lastHp[card] = card.CurrentHp.Value;

                card.CurrentHp.OnValueChanged += _ => HandleHpChanged(card);
                card.HpRegeneration.OnValueChanged += _ =>
                    _view.SetTotalHPRegeneration((int)_cards.Sum(x => x.HpRegeneration));
                card.Attack.OnValueChanged += _ =>
                    _view.SetTotalAttack((int)_cards.Sum(x => x.Attack));
                card.Crit.OnValueChanged += _ =>
                    _view.SetTotalCrit((int)_cards.Sum(x => x.Crit));
                card.CritDmg.OnValueChanged += _ =>
                    _view.SetTotalCritDmg((int)_cards.Sum(x => x.CritDmg));
                card.Block.OnValueChanged += _ =>
                    _view.SetTotalBlock((int)_cards.Sum(x => x.Block));
                card.BlockPower.OnValueChanged += _ =>
                    _view.SetTotalBlockPower((int)_cards.Sum(x => x.BlockPower));
                card.Evade.OnValueChanged += _ =>
                    _view.SetTotalEvade((int)_cards.Sum(x => x.Evade));
                card.MaxHp.OnValueChanged += _ =>
                    _view.SetSliderHp((int)_cards.Sum(x => x.MaxHp), (int)_cards.Sum(x => x.CurrentHp));
            }
        }


        public void SetCardOnToolbar(CardModel card)
        {
            var existCard = _cards.FirstOrDefault(x => x.Id == card.Id);
            
            if (existCard == null)
            {
                _cards.Add(card);
            }
        }

        private void InitTotalStats()
        {
            var totalTeamMaxHp = (int) _cards.Sum(card => card.MaxHp);
            var totalTeamHp = (int) _cards.Sum(card => card.CurrentHp);
            var totalHPs = (int) _cards.Sum(card => card.HpRegeneration);
            var totalAttack = (int) _cards.Sum(card => card.Attack);
            var totalCrit = (int) _cards.Sum(card => card.Crit);
            var totalCritDmg = (int) _cards.Sum(card => card.CritDmg);
            var totalBlock = (int) _cards.Sum(card => card.Block);
            var totalBlockPower = (int) _cards.Sum(card => card.BlockPower);
            var totalEvade = (int) _cards.Sum(card => card.Evade);

            _view.SetTotalHp(totalTeamHp);
            _view.SetTotalHPRegeneration(totalHPs);
            _view.SetTotalAttack(totalAttack);
            _view.SetTotalCrit(totalCrit);
            _view.SetTotalCritDmg(totalCritDmg);
            _view.SetTotalBlock(totalBlock);
            _view.SetTotalBlockPower(totalBlockPower);
            _view.SetTotalEvade(totalEvade);

            _view.SetSliderHp(totalTeamMaxHp, totalTeamHp);
        }

        private void HandleHpChanged(CardModel card)
        {
            var old = _lastHp[card];
            var current = card.CurrentHp.Value;
            _lastHp[card] = current;

            _view.SetTotalHp((int)_cards.Sum(x => x.CurrentHp));

            var duration = current > old ? 1f : 0.5f;
            _view.SetSliderHp(
                (int)_cards.Sum(x => x.MaxHp),
                (int)_cards.Sum(x => x.CurrentHp),
                duration);
        }
    }
}