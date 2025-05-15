using UnityEngine;

namespace Presentation
{
    [DefaultExecutionOrder(-100)] 
    public class UIManager : MonoBehaviour
    {
        public static UIManager I { get; private set; }
        public Transform InventoryContainer, ToolbarContainer;
        void Awake() => I = this;
    }
}