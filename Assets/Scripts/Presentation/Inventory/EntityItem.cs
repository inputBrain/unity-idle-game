using Model.InventoryCard;
using UnityEngine;

namespace Api.Payload
{
    public class EntityItem
    {
        public string DisplayName { get; private set; }
        public Sprite DisplayIcon { get; private set; }
        public IInventoryItem BackingDomainItem { get; private set; } //!!! Ссылка на оригинал модели, которая реализует интерфейс

        public EntityItem(IInventoryItem domainItem, Sprite icon)
        {
            BackingDomainItem = domainItem;
            DisplayName = domainItem.Title;
            DisplayIcon = icon;
        }
    }
}