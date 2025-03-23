using System.Globalization;
using Presentation.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.MVVM.Views
{
    public class BossView : MonoBehaviour
    {
        public Image bossImage;
        public TMP_Text bossHpText;
        public Slider bossHpSlider;

        private BossViewModel _viewModel;

        public void Initialize(BossViewModel viewModel)
        {
            _viewModel = viewModel;
            
            if (_viewModel.Sprite != null)
                bossImage.sprite = _viewModel.Sprite;
            
            UpdateView();
            
            _viewModel.PropertyChanged += (sender, args) => {
                UpdateView();
            };
        }

        private void UpdateView()
        {
            if (_viewModel == null) return;
            
            if (bossHpSlider != null)
            {
                bossHpSlider.value = _viewModel.Hp;
                bossHpSlider.maxValue = _viewModel.MaxHp;
            }
            
            if (bossHpText != null)
            {
                bossHpText.text = _viewModel.Hp.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}