﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model.Card;
using ScriptableObjects.Config;
using UnityEngine;
using Utils;

namespace Services
{
    public class CardLoaderService
    {
        private static IEnumerable<CardScriptableObject> GetCardsObject()
        {
            var loadedCards = ResourceLoadUtils.GetAllScriptableObjects<CardScriptableObject>("/Data/CharacterCards").ToList();
            Debug.Log("Загружено карточек: " + loadedCards.Count);
            return loadedCards;
        }

        public List<CardModel> GetDomainCards()
        {
            return GetCardsObject().Select(so =>
            {
                var card = new CardModel
                {
                    Id = so.Id,
                    Title = so.Title,
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
                    Rarity = { Value = (int)so.Rarity },
                    Count = { Value = 1},
                    DropChance = { Value = so.DropChance}
                };

                return card;
            }).ToList();
        }
    }
}