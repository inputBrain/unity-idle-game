using System;
using ScriptableObjects.Config;
using UnityEngine;

namespace Domain.Entities
{
    public class Card
    {
        public int Id { get; set; }

        public string Title { get; set; }

        private int _level;

        public int Level
        {
            get => _level;
            set
            {
                if (_level == value)
                {
                    return;
                }
                {
                    _level = value;
                    OnLevelChanged?.Invoke(_level);
                }
            }
        }

        public event Action<int> OnLevelChanged;




        private float _expCurrent;

        public float ExpCurrent
        {
            get => _expCurrent;
            set
            {
                if (Mathf.Approximately(_expCurrent, value))
                {
                    return;
                }
                {
                    _expCurrent = value;
                    OnExpCurrentChanged?.Invoke(_expCurrent);
                }
            }
        }

        public event Action<float> OnExpCurrentChanged;


        private float _expToNextLevel;

        public float ExpToNextLevel
        {
            get => _expToNextLevel;
            set
            {
                if (Mathf.Approximately(_expToNextLevel, value))
                {
                    return;
                }
                {
                    _expToNextLevel = value;
                    OnExpToNextLevelChanged?.Invoke(_expToNextLevel);
                }
            }
        }

        public event Action<float> OnExpToNextLevelChanged;


        public float StartBaseExp { get; set; }

        private string _imageResourcesPath;

        public string ImageResourcePath
        {
            get => _imageResourcesPath;
            set
            {
                if (_imageResourcesPath == value)
                {
                    return;
                }
                {
                    _imageResourcesPath = value;
                    OnImageResourcePathChanged?.Invoke(_imageResourcesPath);
                }
            }
        }

        public event Action<string> OnImageResourcePathChanged;



        private float _currentHp;

        public float CurrentHp
        {
            get => _currentHp;
            set
            {
                if (Mathf.Approximately(_currentHp, value)) return;
                
                if (value < 0)
                {
                    _currentHp = 0;
                    return;
                }
                _currentHp = value;
                OnCurrentHpChanged?.Invoke(_currentHp);
            }
        }

        public event Action<float> OnCurrentHpChanged;

        private float _maxHp;

        
        //TODO: value is incorrect
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

        public event Action<float> OnMaxHpChanged;

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

        public event Action<float> OnHpRegenerationChanged;

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

        public event Action<float> OnAttackChanged;

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

        public event Action<float> OnCritChanged;

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

        public event Action<float> OnCritDmgChanged;

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

        public event Action<float> OnBlockChanged;

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

        public event Action<float> OnBlockPowerChanged;

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

        public event Action<float> OnEvadeChanged;

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

        public event Action<int> OnRarityChanged;


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