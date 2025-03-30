using Domain;
using Domain.Entities;

namespace Presentation.MVP.Manager
{
    public class CardInventoryManager : BaseInventoryManager<Card>, IInventoryManager
    {
        public void SelectItem(BaseEntity item)
        {
            if (item is Card card)
            {
                base.SelectItem(card);
            }
        }

        public void RemoveItem(BaseEntity item)
        {
            if (item is Card card)
            {
                base.RemoveItem(card);
            }
        }
    }
}