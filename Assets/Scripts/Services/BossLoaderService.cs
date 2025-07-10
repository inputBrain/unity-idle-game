using System;
using System.Collections.Generic;
using System.Linq;
using Model.Boss;
using Model.Card;
using ScriptableObjects.Config;
using UnityEngine;
using Utils;

namespace Services
{
    public class BossLoaderService
    {
        private static IEnumerable<BossScriptableObject> GetBossesObject()
        {
            var loadedCards = ResourceLoadUtils.GetAllScriptableObjects<BossScriptableObject>("/Data/Boss").ToList();
            Debug.Log("Загружено карточек: " + loadedCards.Count);
            return loadedCards;
        }

        public List<BossModel> GetAllBosses()
        {
            return GetBossesObject().Select(x =>
            {
                var boss = new BossModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    BaseHp = x.BaseHp,
                    BaseAttack = x.BaseAttack,
                    BaseExperienceReward = x.BaseExpReward,
                    BaseGoldReward = x.BaseGoldReward,
                    IconResourcesPath = { Value = $"Boss/{x.Image.name}" },
                    CurrentHp = {Value = x.BaseHp},
                    MaxHp = {Value = x.BaseHp},
                    Attack = {Value = x.BaseAttack},
                    ExpReward = {Value = x.BaseExpReward},
                    GoldReward = {Value = x.BaseGoldReward}
                };

                return boss;
            }).ToList();
        }
    }
}