using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.MVP.Views
{
    public class CardView : MonoBehaviour
    {
        public TMP_Text Level;
        public TMP_Text ExpCurrent;
        public TMP_Text ExpToNextLevel;
        public Image CardImage;


        public void SetImage(string path)
        {
            CardImage.sprite = Resources.Load<Sprite>(path);
        }
    }
}