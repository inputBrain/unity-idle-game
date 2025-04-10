using System;
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
            return GetCardsObject().Select(so =>
            {
                var card = new Card
                {
                    Id = so.Id,
                    Title = so.Title,
                    InstanceId = Guid.NewGuid(),
                    StartBaseExp = so.StartBaseExp,
                    IconResourcesPath = { Value = $"CharacterCards/{so.Image.name}" },
                    Level = { Value = so.Level },
                    ExpCurrent = { Value = so.ExpCurrent },
                    ExpToNextLevel = { Value = so.ExpToNextLevel },
                    CurrentHp = { Value = so.Hp },
                    MaxHp = { Value = so.Hp },
                    HpRegeneration = { Value = so.HpRegeneration },
                    Attack = { Value = so.Attack },
                    Crit = { Value = so.Crit },
                    CritDmg = { Value = so.CritDmg },
                    Block = { Value = so.Block },
                    BlockPower = { Value = so.BlockPower },
                    Evade = { Value = so.Evade },
                    Rarity = { Value = (int)so.Rarity }
                };

                return card;
            }).ToList();
        }
    }
}