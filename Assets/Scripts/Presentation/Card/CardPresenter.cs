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
            _cardView.SetCount(cardModel.Count.Value);
            _cardView.SetRank(_cardModel.Rank.Value);
            UpdateLevel(_cardModel.Level.Value);

            _cardModel.IconResourcesPath.OnValueChanged += UpdateIcon;
            _cardModel.Level.OnValueChanged += UpdateLevel;

            _cardModel.ExpCurrent.OnValueChanged += _cardView.SetSliderCurrentExp;
            _cardModel.ExpToNextLevel.OnValueChanged += _cardView.SetSliderNextExp;

            _cardModel.ExpCurrent.OnValueChanged += UpdateExpText;
            _cardModel.ExpToNextLevel.OnValueChanged += UpdateExpText;

            _cardModel.Count.OnValueChanged += _cardView.SetCount;
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

        private void UpdateExpText(float _)
        {
            if (_cardModel.Level.Value >= CardModel.MaxLevel)
            {
                _cardView.SetMaxLevel();
            }
            else
            {
                _cardView.SetExpText(
                    _cardModel.ExpCurrent.Value,
                    _cardModel.ExpToNextLevel.Value);
            }
        }

        private void UpdateLevel(int level)
        {
            if (level >= CardModel.MaxLevel)
                _cardView.SetMaxLevel();
            else
                _cardView.SetLevel(level);
        }
    }
}