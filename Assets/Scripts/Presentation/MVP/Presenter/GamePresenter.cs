using Domain.Entities;
using Presentation.MVP.Views;

namespace Presentation.MVP.Presenter
{
    public class GamePresenter
    {
        private readonly IGameView _view;
        private readonly Player _model;

        public GamePresenter(IGameView view, Player model)
        {
            _view = view;
            _model = model;

            // Подписываемся на событие изменения монет у игрока
            _model.OnCoinsChanged += OnPlayerCoinsChanged;
        }

        private void OnPlayerCoinsChanged(int coins)
        {
            _view.LogEvent($"[Игровое Событие] Монеток изменилось: {coins}");
        }
    }
}