using Model.Inventory;

namespace Model.User
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Nickname { get; set; }

        public InventoryModel InventoryModel { get; set; }


        public UserModel(int id, string nickname, InventoryModel inventory)
        {
            Id = id;
            Nickname = nickname;
            InventoryModel = inventory;
        }
    }
}