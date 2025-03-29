using System;
using UnityEngine;

namespace Domain.Entities
{
    public class Boss
    {
        public int Id { get; set; }
        public float BaseHp { get; set; }
        public string Title { get; set; }

        private float _currentHp;
        public event Action<float> OnCurrentHpChanged;
        public float CurrentHp
        {
            get => _currentHp;
            set
            {
                if (Mathf.Approximately(_currentHp, value))
                {
                    return;
                }
                
                if (value < 0)
                {
                    _currentHp = 0;
                    return;
                }
                    
                _currentHp = value;
                OnCurrentHpChanged?.Invoke(_currentHp);
            }
        }

        private float _maxHp;
        public event Action<float> OnMaxHpChanged;
        
        public float MaxHp
        {
            get => _maxHp;
            set
            {
                if (Mathf.Approximately(_maxHp, value))
                {
                    return;
                }
                _maxHp = value;
                OnMaxHpChanged?.Invoke(_maxHp);
            }
        }

        private float _attack;
        public event Action<float> OnAttackChanged;
        public float Attack
        {
            get => _attack;
            set
            {
                if (Mathf.Approximately(_attack, value))
                {
                    return;
                }
                _attack = value;
                OnAttackChanged?.Invoke(_attack);
            }
        }


        private int _expReward;
        public event Action<int> OnExpRewardChanged;
        public int ExpReward
        {
            get => _expReward;
            set
            {
                if (_expReward == value)
                {
                    return;
                }
                _expReward = value;
                OnExpRewardChanged?.Invoke(_expReward);
            }
        }

        private int _goldReward;
        public event Action<int> OnGoldRewardChanged;
        public int GoldReward
        {
            get => _goldReward;
            set
            {
                if (_goldReward == value)
                {
                    return;
                }
                _goldReward = value;
                OnGoldRewardChanged?.Invoke(_goldReward);
            }
        }

        private string _imageResourcesPath;
        public event Action<string> OnImageResourcePathChanged;
        public string ImageResourcePath
        {
            get => _imageResourcesPath;
            set
            {
                if (_imageResourcesPath == value)
                {
                    return;
                }
                _imageResourcesPath = value;
                OnImageResourcePathChanged?.Invoke(_imageResourcesPath);
            }
        }
        

        public void GetUpdatedStats(int zone)
        {
            MaxHp = BaseHp * zone;
            CurrentHp = MaxHp;
            Attack += 100f;
        }
    }
}