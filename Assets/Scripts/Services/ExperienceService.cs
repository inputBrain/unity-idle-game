using System;
using TMPro;
using UnityEngine.UI;

namespace Services
{
    public class ExperienceService : Singleton<ExperienceService>
    {
        public Slider Card1ExpSlider;
        public Slider Card2ExpSlider;
        public Slider Card3ExpSlider;
        public Slider Card4ExpSlider;
        public Slider Card5ExpSlider;

        public TMP_Text Card1TextOnSlider;
        public TMP_Text Card2TextOnSlider;
        public TMP_Text Card3TextOnSlider;
        public TMP_Text Card4TextOnSlider;
        public TMP_Text Card5TextOnSlider;
        
        private float card1CurrentExp;
        private float card2CurrentExp;
        private float card3CurrentExp;
        private float card4CurrentExp;
        private float card5CurrentExp;



        private void Start()
        {
            throw new NotImplementedException();
        }
    }
}