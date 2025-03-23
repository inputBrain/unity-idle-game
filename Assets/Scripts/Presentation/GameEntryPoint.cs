using Domain.Entities;
using Presentation.MVP.Presenter;
using Presentation.MVP.Views;
using UnityEngine;

namespace Presentation
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Header("Ссылка на View")]
        public PlayerView playerView;
        public GameView gameView;
        
        private Player _player = new() { Coins = 100 };

        private void Awake()
        {
            var playerPresenter = new PlayerPresenter(playerView, _player);
            var gamePresenter = new GamePresenter(gameView, _player);
            this.InvokeRepeating(nameof(AddCoin), 1f, 1f);
        }

        void AddCoin()
        {
            _player.Coins += 1;
        }
    }
}