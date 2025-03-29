using System;
using UnityEngine;

namespace Domain.Entities
{
    public class Boss
    {
        public int Id { get; set; }
        public string Title { get; set; }

        private float _hp;
        public event Action<float> OnHpChanged;
        public float Hp
        {
            get => _hp;
            set
            {
                if (Mathf.Approximately(_hp, value))
                {
                    return;
                }
                _hp = value;
                OnHpChanged?.Invoke(_hp);
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

        private float _startAttack;
        public event Action<float> OnStartAttackChanged;
        public float StartAttack
        {
            get => _startAttack;
            set
            {
                if (Mathf.Approximately(_startAttack, value))
                {
                    return;
                }
                _startAttack = value;
                OnStartAttackChanged?.Invoke(_startAttack);
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


        public float TakeDamage(float incomingDamage)
        {
            var effectiveDamage = Mathf.Max(incomingDamage, 0);
            Hp = Mathf.Max(Hp - effectiveDamage, 0);

            return effectiveDamage;
        }


        public void UpgradeForNextZone(int zoneNumber)
        {
            const int MaxHpIncreasePerZone = 20;
            const int AttackIncreasePerZone = 5;

            MaxHp += MaxHpIncreasePerZone * zoneNumber;
            Hp = MaxHp;
            Attack = StartAttack + AttackIncreasePerZone * zoneNumber;
        }
    }
}