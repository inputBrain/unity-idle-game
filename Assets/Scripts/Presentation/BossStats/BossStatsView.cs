using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.BossStats
{
    public class BossStatsView : MonoBehaviour
    {
        public TMP_Text Attack;

        public TMP_Text ExperienceReward;

        public TMP_Text GoldReward;
        
        public Image BossImage;
        
        
        public void SetAttack(float value)
        {
            Attack.text = "Attack: " + value;
        }

        public void SetExperienceReward(float value)
        {
            ExperienceReward.text = "Experience: " + value;
        }
        
        public void SetGoldReward(float value)
        {
            GoldReward.text = "Gold: " + value;
        }

        
        public void SetImage(string path)
        {
            BossImage.sprite = Resources.Load<Sprite>(path);
        }
    }
}