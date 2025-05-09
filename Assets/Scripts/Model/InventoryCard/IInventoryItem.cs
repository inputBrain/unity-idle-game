using System;

namespace Model.InventoryCard
{
    public interface IInventoryItem
    {
        int Id { get; }
        string Title { get; }
        ReactiveProperty<string> IconResourcesPath { get; }
        public ReactiveProperty<int> Count { get; }
        
        

        Guid InstanceId { get; }
    }
}