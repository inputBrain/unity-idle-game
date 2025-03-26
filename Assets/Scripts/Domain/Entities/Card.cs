using ScriptableObjects.Config;
using UnityEngine;

namespace Domain.Entities
{
    public class Card
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public float ExpCurrent { get; set; }

        public float ExpToNextLevel { get; set; }

        public float StartBaseExp { get; set; }

        public string ImageResourcePath { get; set; }


        private float _currentHp;

        public float CurrentHp
        {
            get => _currentHp;
            set
            {
                if (Mathf.Approximately(_currentHp, value))
                {
                    return;
                }
                {
                    _currentHp = value;
                    OnCurrentHpChanged?.Invoke(_currentHp);
                }
            }
        }

        public event System.Action<float> OnCurrentHpChanged;

        private float _maxHp;

        public float MaxHp
        {
            get => _maxHp;
            set
            {
                if (Mathf.Approximately(_maxHp, value))
                    return;

                _maxHp = value;
                OnMaxHpChanged?.Invoke(_maxHp);
            }
        }

        public event System.Action<float> OnMaxHpChanged;

        private float _hpRegeneration;

        public float HpRegeneration
        {
            get => _hpRegeneration;
            set
            {
                if (Mathf.Approximately(_hpRegeneration, value))
                    return;

                _hpRegeneration = value;
                OnHpRegenerationChanged?.Invoke(_hpRegeneration);
            }
        }

        public event System.Action<float> OnHpRegenerationChanged;

        private float _attack;

        public float Attack
        {
            get => _attack;
            set
            {
                if (Mathf.Approximately(_attack, value))
                    return;

                _attack = value;
                OnAttackChanged?.Invoke(_attack);
            }
        }

        public event System.Action<float> OnAttackChanged;

        private float _crit;

        public float Crit
        {
            get => _crit;
            set
            {
                if (Mathf.Approximately(_crit, value))
                    return;

                _crit = value;
                OnCritChanged?.Invoke(_crit);
            }
        }

        public event System.Action<float> OnCritChanged;

        private float _critDmg;

        public float CritDmg
        {
            get => _critDmg;
            set
            {
                if (Mathf.Approximately(_critDmg, value))
                    return;

                _critDmg = value;
                OnCritDmgChanged?.Invoke(_critDmg);
            }
        }

        public event System.Action<float> OnCritDmgChanged;

        private float _block;

        public float Block
        {
            get => _block;
            set
            {
                if (Mathf.Approximately(_block, value))
                    return;

                _block = value;
                OnBlockChanged?.Invoke(_block);
            }
        }

        public event System.Action<float> OnBlockChanged;

        private float _blockPower;

        public float BlockPower
        {
            get => _blockPower;
            set
            {
                if (Mathf.Approximately(_blockPower, value))
                    return;

                _blockPower = value;
                OnBlockPowerChanged?.Invoke(_blockPower);
            }
        }

        public event System.Action<float> OnBlockPowerChanged;

        private float _evade;

        public float Evade
        {
            get => _evade;
            set
            {
                if (Mathf.Approximately(_evade, value))
                    return;

                _evade = value;
                OnEvadeChanged?.Invoke(_evade);
            }
        }

        public event System.Action<float> OnEvadeChanged;

        private int _rarity;

        public int Rarity
        {
            get => _rarity;
            set
            {
                if (_rarity == value)
                    return;

                _rarity = value;
                OnRarityChanged?.Invoke(_rarity);
            }
        }

        public event System.Action<int> OnRarityChanged;


        public void TakeDamage(float damage)
        {
            CurrentHp -= damage;
            if (CurrentHp < 0)
                CurrentHp = 0;
        }


        public void ApplyLevelUpBonus()
        {
            MaxHp *= 1.2f;
            Attack *= 1.1f;
            CurrentHp = MaxHp;
        }
    }
}