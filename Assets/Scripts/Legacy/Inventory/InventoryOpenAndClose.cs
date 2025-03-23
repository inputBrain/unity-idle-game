using UnityEngine;

namespace OLd.Inventory
{
    public class InventoryOpenAndClose : MonoBehaviour
    {
        public GameObject InventoryCanvas;

        private bool _isOpenedInventory;

        public void Awake()
        {
            _isOpenedInventory = false;
        }

        public void ToggleInventory()
        {
            if (_isOpenedInventory)
            {
                _onCloseInventory();
            }
            else
            {
                _onOpenInventory();
            }
        }


        private void _onOpenInventory()
        {
            InventoryCanvas.SetActive(true);
            _isOpenedInventory = true;
        }


        private void _onCloseInventory()
        {
            InventoryCanvas.SetActive(false);
            _isOpenedInventory = false;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }
    }
}