using System;

namespace Presentation.MVP.Views
{
    public interface IPlayerView
    {
        void SetCoins(int coins);
        event Action OnAddCoinClicked;
    }
}