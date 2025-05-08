using System;

namespace Model.InventoryCard
{
    public interface IInventoryItem
    {
        int Id { get; }
        string Title { get; }
        ReactiveProperty<string> IconResourcesPath { get; }
        Guid InstanceId { get; }
    }
}