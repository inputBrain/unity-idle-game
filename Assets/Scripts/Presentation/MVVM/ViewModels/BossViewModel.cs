using Domain.Entities;
using UnityEngine;

namespace Presentation.MVVM.ViewModels
{
    public class BossViewModel : BaseViewModel
    {
        private readonly Boss _model;
        private Sprite _sprite;

        public int Id => _model.Id;
        
        private string _title;
        public string Title
        {
            get => _title;
            private set => this.SetProperty(ref _title, value);
        }
        
        private float _hp;
        public float Hp
        {
            get => _hp;
            private set => this.SetProperty(ref _hp, value);
        }
        
        private float _maxHp;
        public float MaxHp
        {
            get => _maxHp;
            private set => this.SetProperty(ref _maxHp, value);
        }
        
        private float _armor;
        public float Armor
        {
            get => _armor;
            private set => this.SetProperty(ref _armor, value);
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

        public BossViewModel(Boss boss)
        {
            _model = boss;
            UpdateView(); // Первоначальное обновление представления
            LoadSprite();
        }

        // Обновляет View-модель на основе изменений в модели
        public void UpdateView()
        {
            Title = _model.Title;
            Hp = _model.Hp;
            MaxHp = _model.MaxHp;
            Armor = _model.Armor;
            Attack = _model.Attack;
        }
        
        private void LoadSprite()
        {
            // Загрузка спрайта - это логика представления, поэтому она остается в ViewModel
            Sprite sprite = Resources.Load<Sprite>(_model.ImageResourcePath);
            if (sprite != null)
                Sprite = sprite;
            else
                Debug.LogWarning($"Не удалось загрузить спрайт для босса по пути: {_model.ImageResourcePath}");
        }
    }
}