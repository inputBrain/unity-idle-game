using System;
using System.Collections.Generic;
using Model.Card;
using Model.InventoryCard;
using Presentation.Entity;
using Presentation.Inventory;
using UnityEngine;

namespace Presentation.Toolbar
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private Transform toolbarContainer;
        [SerializeField] private EntityView itemPrefab;

        public void ClearAllItems()
        {
            foreach (Transform t in toolbarContainer) Destroy(t.gameObject);
        }

        public EntityView SpawnItemView()
            => Instantiate(itemPrefab, toolbarContainer, false);
    }

}