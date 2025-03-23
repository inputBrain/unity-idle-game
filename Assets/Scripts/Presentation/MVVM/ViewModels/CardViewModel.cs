using Domain.Entities;
using UnityEngine;

namespace Presentation.MVVM.ViewModels
{
    public class CardViewModel : BaseViewModel
    {
        private readonly Card _model;
        private Sprite _sprite;

        public int Id => _model.Id;
        
        private string _title;
        public string Title
        {
            get => _title;
            private set => this.SetProperty(ref _title, value);
        }
        
        private int _level;
        public int Level
        {
            get => _level;
            private set => this.SetProperty(ref _level, value);
        }
        
        private float _expCurrent;
        public float ExpCurrent
        {
            get => _expCurrent;
            private set => this.SetProperty(ref _expCurrent, value);
        }
        
        private float _expToNextLevel;
        public float ExpToNextLevel
        {
            get => _expToNextLevel;
            private set => this.SetProperty(ref _expToNextLevel, value);
        }
        
        private float _currentHp;
        public float CurrentHp
        {
            get => _currentHp;
            private set => this.SetProperty(ref _currentHp, value);
        }
        
        private float _maxHp;
        public float MaxHp
        {
            get => _maxHp;
            private set => this.SetProperty(ref _maxHp, value);
        }
        
        private float _attack;
        public float Attack
        {
            get => _attack;
            private set => this.SetProperty(ref _attack, value);
        }

        public Sprite Sprite
        {
            get => _sprite;
            private set => this.SetProperty(ref _sprite, value);
        }

        // Вычисляемое свойство для UI
        public float ExpPercentage => _expToNextLevel > 0 ? _expCurrent / _expToNextLevel : 0;

        public CardViewModel(Card card)
        {
            _model = card;
            UpdateView(); // Первоначальное обновление представления
        }

        // Обновляет View-модель на основе изменений в модели
        public void UpdateView()
        {
            Title = _model.Title;
            Level = _model.Level;
            ExpCurrent = _model.ExpCurrent;
            ExpToNextLevel = _model.ExpToNextLevel;
            CurrentHp = _model.CurrentHp;
            MaxHp = _model.MaxHp;
            Attack = _model.Attack;
            
            this.OnPropertyChanged(nameof(ExpPercentage));
        }
        
    }
}