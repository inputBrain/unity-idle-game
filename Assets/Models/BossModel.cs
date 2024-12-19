using System;
using Battle;
using UnityEngine.UIElements;

namespace Models
{
    public class BossModel
    {
        public int Id;

        public string Title;

        // public float Armor;

        public float CurrentHP;
        public float MaxHp;

        public float Attack;
        public int ExpReward;
        public int GoldReward;

        public Image Image;
        
        
        
        public event Action<float, float> OnHPChangedAction;

        public event Action<float, float, float> OnBossDefeat;
        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                MaxHp *= 1.05f;
                Attack *= 1.01f;
                ExpReward += 10;
                GoldReward += 1;

                CurrentHP  = MaxHp;
                OnHPChangedAction?.Invoke(CurrentHP, MaxHp);
                OnBossDefeat.Invoke(Attack, ExpReward, GoldReward);
                
            }
            else
            {
                OnHPChangedAction?.Invoke(CurrentHP, MaxHp);
            }
        }
        
        public void InvokeHealthChanged()
        {
            OnHPChangedAction?.Invoke((int)CurrentHP, (int)MaxHp);
        }


    }
}