using Domain.Entities;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation.MVP.Presenter
{
    public class CardPresenter : MonoBehaviour
    {
        private readonly Card _card;

        private readonly CardView _cardView;


        public CardPresenter(Card card, CardView cardView)
        {
            _card = card;
            _cardView = cardView;
            
            UpdateImage(card.ImageResourcePath);
            _card.OnImageResourcePathChanged += UpdateImage;
            _card.OnLevelChanged += _cardView.SetLevel;
        }


        private void UpdateImage(string path)
        {
            _cardView.SetImage(path);
        }
    }
}