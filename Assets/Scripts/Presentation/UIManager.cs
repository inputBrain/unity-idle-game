using UnityEngine;

namespace Presentation
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager I { get; private set; }
        public Transform InventoryContainer, ToolbarContainer;
        void Awake() => I = this;
    }
}