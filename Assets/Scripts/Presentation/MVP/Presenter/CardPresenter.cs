using Domain.Entities;
using Presentation.MVP.Views;

namespace Presentation.MVP.Presenter
{
    public class CardPresenter
    {
        private Card _card;
        private CardView _cardView;

        public void Init(Card card, CardView cardView)
        {
            _card = card;
            _cardView = cardView;
            
            UpdateImage(card.ImageResourcesPath.Value);
            UpdateSlider(card.ExpCurrent, card.ExpToNextLevel);
            
            _card.ImageResourcesPath.OnValueChanged += UpdateImage;
            _card.Level.OnValueChanged += _cardView.SetLevel;
            
            _card.ExpCurrent.OnValueChanged += _cardView.SetSliderCurrentExp;
            _card.ExpToNextLevel.OnValueChanged += _cardView.SetSliderNextExp;
            
            _card.ExpCurrent.OnValueChanged += _cardView.SetTextCurrentExp;
            _card.ExpToNextLevel.OnValueChanged += _cardView.SetTextNextExp;
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