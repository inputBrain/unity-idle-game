using TMPro;
using UnityEngine;

namespace Presentation.MVP.Views
{
    public class ZoneView : MonoBehaviour
    {
        public TMP_Text ZoneText;
        
        public void UpdateZoneText(int zone)
        {
            ZoneText.text = $"Zone: {zone}";
        }
    }
}