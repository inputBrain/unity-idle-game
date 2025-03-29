﻿using System.Collections.Generic;
using System.Globalization;
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
            var totalTeamHp = Mathf.RoundToInt(_cards.Sum(card => card.CurrentHp));

            var totalHPs = Mathf.RoundToInt(_cards.Sum(card => card.HpRegeneration));
            var totalAttack = Mathf.RoundToInt(_cards.Sum(card => card.Attack));
            var totalCrit = Mathf.RoundToInt(_cards.Sum(card => card.Crit));
            var totalCritDmg = Mathf.RoundToInt(_cards.Sum(card => card.CritDmg));
            var totalBlock = Mathf.RoundToInt(_cards.Sum(card => card.Block));
            var totalBlockPower = Mathf.RoundToInt(_cards.Sum(card => card.BlockPower));
            var totalEvade = Mathf.RoundToInt(_cards.Sum(card => card.Evade));

            _view.SetTotalHp(totalTeamHp);
            _view.SetTotalHPs(totalHPs);
            _view.SetTotalAttack(totalAttack);
            _view.SetTotalCrit(totalCrit);
            _view.SetTotalCritDmg(totalCritDmg);
            _view.SetTotalBlock(totalBlock);
            _view.SetTotalBlockPower(totalBlockPower);
            _view.SetTotalEvade(totalEvade);


            var preparedHpToDispalyOnSlider = totalTeamHp.ToString(CultureInfo.InvariantCulture);

            _view.SetSliderHp(totalTeamHp, preparedHpToDispalyOnSlider);
        }
    }
}