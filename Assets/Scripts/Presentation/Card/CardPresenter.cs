using Model.Card;

namespace Presentation.Card
{
    public class CardPresenter
    {
        private CardModel _cardModel;
        private CardView _cardView;

        public void Init(CardModel cardModel, CardView cardView)
        {
            _cardModel = cardModel;
            _cardView = cardView;
            
            UpdateIcon(cardModel.IconResourcesPath.Value);
            UpdateSlider(cardModel.ExpCurrent, cardModel.ExpToNextLevel);
            
            _cardModel.IconResourcesPath.OnValueChanged += UpdateIcon;
            _cardModel.Level.OnValueChanged += _cardView.SetLevel;
            
            _cardModel.ExpCurrent.OnValueChanged += _cardView.SetSliderCurrentExp;
            _cardModel.ExpToNextLevel.OnValueChanged += _cardView.SetSliderNextExp;
            
            _cardModel.ExpCurrent.OnValueChanged += _cardView.SetTextCurrentExp;
            _cardModel.ExpToNextLevel.OnValueChanged += _cardView.SetTextNextExp;
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