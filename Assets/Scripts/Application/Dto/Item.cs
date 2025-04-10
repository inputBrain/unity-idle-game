using Domain.Interfaces;
using UnityEngine;

namespace Application.Dto
{
    public class Item
    {
        public string DisplayName { get; private set; }
        public Sprite DisplayIcon { get; private set; }
        public IInventoryItem BackingDomainItem { get; private set; } //!!! Ссылка на оригинал модели, которая реализует интерфейс

        public Item(IInventoryItem domainItem, Sprite icon)
        {
            BackingDomainItem = domainItem;
            DisplayName = domainItem.Title;
            DisplayIcon = icon;
        }
    }
}