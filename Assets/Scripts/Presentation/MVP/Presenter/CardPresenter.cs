using System;
using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class CardPresenter : MonoBehaviour
    {
        private Card _card;
        private CardView _cardView;

        public void Init(Card card, CardView cardView)
        {
            _card = card;
            _cardView = cardView;
            
            UpdateImage(card.ImageResourcePath);
            UpdateSlider(card.ExpCurrent, card.ExpToNextLevel);
            
            _card.OnImageResourcePathChanged += UpdateImage;
            _card.OnLevelChanged += _cardView.SetLevel;
            
            _card.OnExpCurrentChanged += _cardView.SetSliderCurrentExp;
            _card.OnExpToNextLevelChanged += _cardView.SetSliderNextExp;
            
            _card.OnExpCurrentChanged += _cardView.SetTextCurrentExp;
            _card.OnExpToNextLevelChanged += _cardView.SetTextNextExp;
        }


        private void UpdateImage(string path)
        {
            _cardView.SetImage(path);
        }


        private void UpdateSlider(float a, float b)
        {
            _cardView.SetSliderCurrentExp(a);
            _cardView.SetSliderNextExp(b);
        }
    }
}