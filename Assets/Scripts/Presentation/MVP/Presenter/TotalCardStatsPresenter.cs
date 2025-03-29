using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class TotalCardStatsPresenter
    {
        private TotalCardStatsView _view;
        private List<Card> _cards;
        
        public void Init(List<Card> cards, TotalCardStatsView view)
        {
            _cards = cards;
            _view = view;
            
            InitTotalStats();
    
            foreach (var card in _cards)
            {
                card.OnCurrentHpChanged += _ =>       _view.SetTotalHp((int)_cards.Sum(x => x.CurrentHp));
                card.OnHpRegenerationChanged += _ =>  _view.SetTotalHPRegeneration((int)_cards.Sum(x => x.HpRegeneration));
                card.OnAttackChanged += _ =>          _view.SetTotalAttack((int)_cards.Sum(x => x.Attack));
                card.OnCritChanged += _ =>            _view.SetTotalCrit((int)_cards.Sum(x => x.Crit));
                card.OnCritDmgChanged += _ =>         _view.SetTotalCritDmg((int)_cards.Sum(x => x.CritDmg));
                card.OnBlockChanged += _ =>           _view.SetTotalBlock((int)_cards.Sum(x => x.Block));
                card.OnBlockPowerChanged += _ =>      _view.SetTotalBlockPower((int)_cards.Sum(x => x.BlockPower));
                card.OnEvadeChanged += _ =>           _view.SetTotalEvade((int)_cards.Sum(x => x.Evade));
                card.OnMaxHpChanged += _ =>           _view.SetSliderHp((int) _cards.Sum(x => x.MaxHp), (int) _cards.Sum(x => x.CurrentHp));
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
    }
}