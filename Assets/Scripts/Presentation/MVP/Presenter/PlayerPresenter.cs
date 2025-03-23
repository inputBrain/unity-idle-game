using Domain.Entities;
using Presentation.MVP.Views;

namespace Presentation.MVP.Presenter
{
    public class PlayerPresenter
    {
        private readonly IPlayerView _view;
        private readonly Player _model;

        public PlayerPresenter(IPlayerView view, Player model)
        {
            _view = view;
            _model = model;

            view.OnAddCoinClicked += AddCoin;
            model.OnCoinsChanged += view.SetCoins;

            view.SetCoins(model.Coins);
        }

        private void AddCoin()
        {
            _model.Coins++;
        }
    }

}