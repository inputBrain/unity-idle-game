using System;
using UnityEngine;

namespace Domain.Interfaces
{
    public interface IInventoryItem
    {
        int Id { get; }
        string Title { get; }
        ReactiveProperty<string> IconResourcesPath { get; }
        Guid InstanceId { get; }
    }
}