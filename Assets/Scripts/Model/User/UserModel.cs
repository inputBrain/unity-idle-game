using System.Collections.Generic;
using Model.Card;
using Model.Inventory;
using NUnit.Framework;

namespace Model.User
{
    public class UserModel
    {
        public int Id { get; set; }
        
        public InventoryModel InventoryModel { get; set; }
        
        public List<CardModel> Cards { get; set; }
    }
}