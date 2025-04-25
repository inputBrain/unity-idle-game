namespace Presentation.Inventory
{
    public class ItemPresenter
    {
        private Domain.Entities.Card _card;
        private ItemView _view;

        public void Init(Domain.Entities.Card card, ItemView view)
        {
            _card = card;
            _view = view;

            UpdateCount(card.Count.Value);
            _card.Count.OnValueChanged += UpdateCount;
        }

        private void UpdateCount(int count)
        {
            _view.SetCountText(count > 1 ? $"x{count}" : "", count > 1);
        }
    }

}