 using ScriptableObjects;
 using TMPro;
 using UnityEngine;
 using UnityEngine.UI;

 public class InventoryItem : MonoBehaviour
{

    public Button ConjureButton;

    public Image  Renderer;

    public TextMeshProUGUI QuantityText;

    [HideInInspector]
    public Item Item;

}
