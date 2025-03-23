using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using ScriptableObjects.Config;
using UnityEngine;
using Utils;

namespace Presentation.Services
{
    public class CardLoaderService
    {
        private static IEnumerable<CardObject> GetCardsObject()
        {
            var loadedCards = ResourceLoadUtils.GetAllScriptableObjects<CardObject>("/Data/CharacterCards").ToList();
            Debug.Log("Загружено карточек: " + loadedCards.Count);
            return loadedCards;
        }

        public List<Card> GetDomainCards()
        {
            return GetCardsObject().Select(so => new Card
            {
                Id = so.Id,
                Title = so.Title,
                Level = so.Level,
                ExpCurrent = so.ExpCurrent,
                ExpToNextLevel = so.ExpToNextLevel,
                StartBaseExp = so.StartBaseExp,
                CurrentHp = so.Hp,
                MaxHp = so.Hp,
                HpRegeneration = so.HpRegeneration,
                Attack = so.Attack,
                Crit = so.Crit,
                CritDmg = so.CritDmg,
                Block = so.Block,
                BlockPower = so.BlockPower,
                Evade = so.Evade,
                Rarity = (int)so.Rarity,
                ImageResourcePath = $"CharacterCards/{so.Image.name}"
            }).ToList();
        }
    }
}