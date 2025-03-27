using System.Globalization;
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
        public Slider Slider;

        
        public void SetLevel(int level)
        {
            Level.text = "lvl: " + level;
        }
        
        public void SetSliderCurrentExp(float currentExp)
        {
            Slider.value = currentExp;
        }
        
        public void SetSliderNextExp(float nextExp)
        {
            Slider.maxValue = nextExp;
        }
        
        public void SetTextNextExp(float nextExp)
        {
            ExpToNextLevel.text = "/" +  (int)nextExp;
        }
        
        public void SetTextCurrentExp(float currentExp)
        {
            
            ExpCurrent.text = ((int)currentExp).ToString(CultureInfo.InvariantCulture);
        }



        public void SetImage(string path)
        {
            CardImage.sprite = Resources.Load<Sprite>(path);
        }
    }
}