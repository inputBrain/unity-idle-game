using System.Linq;
using System.Threading.Tasks;
using Model.Card;
using Model.Inventory;
using Model.InventoryCard;
using Model.User;
using Services;

namespace API
{
    public class FakeUserService : IUserService
    {
        private readonly CardLoaderService _cardLoader;


        public FakeUserService()
        {
            _cardLoader = new CardLoaderService();
        }


        public Task<UserModel> GetCurrentUserAsync()
        {
            var allCards = _cardLoader.GetDomainCards();
            
            var selected = allCards.Where(x => x.Id is 1 or 2 or 3).Select(x => new CardModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    IconResourcesPath = { Value = x.IconResourcesPath.Value},
                    Level = { Value = x.Level.Value},
                    ExpCurrent = { Value = x.ExpCurrent.Value },
                    ExpToNextLevel = { Value = x.ExpToNextLevel.Value },
                    CurrentHp = { Value = x.CurrentHp.Value },
                    MaxHp = { Value = x.MaxHp.Value },
                    HpRegeneration = { Value = x.HpRegeneration.Value },
                    Attack = { Value = x.Attack.Value },
                    Crit = { Value = x.Crit.Value },
                    CritDmg = { Value = x.CritDmg.Value },
                    Block = { Value = x.Block.Value },
                    BlockPower = { Value = x.BlockPower.Value },
                    Evade = { Value = x.Evade.Value },
                    Rarity = { Value = x.Rarity.Value },
                    Count = { Value = x.Count.Value },
                    DropChance = { Value = x.DropChance.Value }
                }
            ).Cast<IInventoryItem>().ToList();

            var inventory = new InventoryModel();
            inventory.LoadItems(selected);


            var user = new UserModel(id: 0, nickname: "Player 0", inventory: inventory);

            return Task.FromResult(user);
        }
    }
}