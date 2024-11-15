 using ScriptableObjects;
 using TMPro;
 using UnityEditor;
 using UnityEngine;
 using UnityEngine.UI;

 public class InventoryItem : MonoBehaviour
{

    public Button ConjureButton;

    public SpriteRenderer Renderer;

    public TextMeshProUGUI QuantityText;

    [HideInInspector]
    public Item Item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
