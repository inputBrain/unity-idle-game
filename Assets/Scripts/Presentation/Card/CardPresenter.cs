namespace Presentation.Card
{
    public class CardPresenter
    {
        private Domain.Entities.Card _card;
        private CardView _cardView;

        public void Init(Domain.Entities.Card card, CardView cardView)
        {
            _card = card;
            _cardView = cardView;
            
            UpdateIcon(card.IconResourcesPath.Value);
            UpdateSlider(card.ExpCurrent, card.ExpToNextLevel);
            
            _card.IconResourcesPath.OnValueChanged += UpdateIcon;
            _card.Level.OnValueChanged += _cardView.SetLevel;
            
            _card.ExpCurrent.OnValueChanged += _cardView.SetSliderCurrentExp;
            _card.ExpToNextLevel.OnValueChanged += _cardView.SetSliderNextExp;
            
            _card.ExpCurrent.OnValueChanged += _cardView.SetTextCurrentExp;
            _card.ExpToNextLevel.OnValueChanged += _cardView.SetTextNextExp;
        }


        private void UpdateIcon(string path)
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