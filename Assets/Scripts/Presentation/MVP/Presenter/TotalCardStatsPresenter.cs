using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class TotalCardStatsPresenter : MonoBehaviour
    {
        private readonly TotalCardStatsView _view;
        private readonly List<Card> _cards;

        public TotalCardStatsPresenter(List<Card> cards, TotalCardStatsView view)
        {
            _cards = cards;
            _view = view;

            UpdateTotalStats();

            foreach (var card in _cards)
            {
                card.OnCurrentHpChanged += _ => UpdateTotalStats();
                card.OnMaxHpChanged += _ => UpdateTotalStats();
                card.OnHpRegenerationChanged += _ => UpdateTotalStats();
                card.OnAttackChanged += _ => UpdateTotalStats();
                card.OnCritChanged += _ => UpdateTotalStats();
                card.OnCritDmgChanged += _ => UpdateTotalStats();
                card.OnBlockChanged += _ => UpdateTotalStats();
                card.OnBlockPowerChanged += _ => UpdateTotalStats();
                card.OnEvadeChanged += _ => UpdateTotalStats();
            }
        }

        private void UpdateTotalStats()
        {
            _view.SetTotalHp(_cards.Sum(card => card.CurrentHp));
            _view.SetTotalHPs(_cards.Sum(card => card.HpRegeneration));
            _view.SetTotalAttack(_cards.Sum(card => card.Attack));
            _view.SetTotalCrit(_cards.Sum(card => card.Crit));
            _view.SetTotalCritDmg(_cards.Sum(card => card.CritDmg));
            _view.SetTotalBlock(_cards.Sum(card => card.Block));
            _view.SetTotalBlockPower(_cards.Sum(card => card.BlockPower));
            _view.SetTotalEvade(_cards.Sum(card => card.Evade));
        }
    }
}