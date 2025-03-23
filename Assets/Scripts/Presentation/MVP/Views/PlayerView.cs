using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.MVP.Views
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        public TMP_Text coinsText;
        public Button addCoinButton;

        public event Action OnAddCoinClicked;

        private void Awake()
        {
            addCoinButton.onClick.AddListener(() =>
            {
                OnAddCoinClicked?.Invoke();
            });
        }

        public void SetCoins(int coins)
        {
            coinsText.text = "Монеток: " + coins;
        }
    }
}