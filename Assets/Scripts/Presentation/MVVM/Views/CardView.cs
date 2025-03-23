using System.Globalization;
using Presentation.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.MVVM.Views
{
    public class CardView : MonoBehaviour
    {
        public Image cardImage;
        public Slider expSlider;
        public Slider hpSlider;
        public TMP_Text hpText;
        public TMP_Text expTextOnSlider;
        public TMP_Text levelText;

        private CardViewModel _viewModel;

        public void Initialize(CardViewModel viewModel)
        {
            _viewModel = viewModel;
            
            if (_viewModel.Sprite != null)
                cardImage.sprite = _viewModel.Sprite;
            
            UpdateView();
            
            _viewModel.PropertyChanged += (sender, args) => {
                UpdateView();
            };
        }

        private void UpdateView()
        {
            if (_viewModel == null) return;
            
            if (hpSlider != null)
            {
                hpSlider.value = _viewModel.CurrentHp;
                hpSlider.maxValue = _viewModel.MaxHp;
            }
            
            if (hpText != null)
            {
                hpText.text = _viewModel.CurrentHp.ToString(CultureInfo.InvariantCulture);
            }
            
            if (expSlider != null)
            {
                expSlider.value = _viewModel.ExpPercentage;
            }
            
            if (expTextOnSlider != null)
            {
                expTextOnSlider.text = $"{_viewModel.ExpCurrent}/{_viewModel.ExpToNextLevel}";
            }
            
            if (levelText != null)
            {
                levelText.text = _viewModel.Level.ToString();
            }
        }
    }
}