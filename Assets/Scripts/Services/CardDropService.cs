using System.Collections.Generic;
using System.Linq;
using Model.Card;

namespace Services
{
    public class CardDropService
    {
        private readonly List<CardModel> _availableCards;

        public CardDropService(List<CardModel> availableCards)
        {
            _availableCards = availableCards;
        }

        public CardModel RollDrop()
        {
            var totalChance = _availableCards.Sum(card => card.DropChance.Value);
            var roll = UnityEngine.Random.value * totalChance;

            var current = 0f;
            foreach (var card in _availableCards)
            {
                current += card.DropChance.Value;
                if (roll <= current)
                {
                    return card;
                }
            }

            return null;
        }

        
        public List<CardModel> RollMultipleDrops(int count)
        {
            var drops = new List<CardModel>();
            for (var i = 0; i < count; i++)
            {
                var droppedCard = RollDrop();
                if (droppedCard != null)
                {
                    var clone = new CardLoaderService().GetDomainCards().FirstOrDefault(c => c.Id == droppedCard.Id);
                    if (clone != null)
                        drops.Add(clone);
                }
            }

            return drops;
        }
    }

}