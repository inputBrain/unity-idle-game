// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Application.Dto;
// using Domain.Entities;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = UnityEngine.Random;
//
// namespace OLd.Battle
// {
//     public class BattleScript : MonoBehaviour
//     {
//         public List<Card> CardList = new();
//         public TMP_Text Zone_Text;
//         public int Zone = 1;
//
//         public TotalCardStatsDto TotalCardStat = new();
//         public Boss Boss = new();
//
//         public Boss Enemy = new();
//
//
//         public Slider EnemyHP_Slider;
//         public TMP_Text EnemyHP_TextOnSlider;
//
//         public Slider TeamHP_Slider;
//         public TMP_Text TeamHP_TextOnSlider;
//
//         public TMP_Text HP_Text;
//         public TMP_Text HPs_Text;
//         public TMP_Text Attack_Text;
//         public TMP_Text Crit_Text;
//         public TMP_Text CritDmg_Text;
//         public TMP_Text Block_Text;
//         public TMP_Text BlockPower_Text;
//         public TMP_Text Evade_Text;
//
//         public TMP_Text EnemyType_Text;
//         public TMP_Text EnemyAttack_Text;
//         public TMP_Text EnemyExpReward_Text;
//         public TMP_Text EnemyGoldReward_Text;
//
//         private float enemyCurrentHP;
//         private float teamCurrentHP;
//         
//         
//         public Slider Card1Exp_Slider;
//         public Slider Card2Exp_Slider;
//         public Slider Card3Exp_Slider;
//         public Slider Card4Exp_Slider;
//         public Slider Card5Exp_Slider;
//
//         public TMP_Text Card1EXP_TextOnSlider;
//         public TMP_Text Card2EXP_TextOnSlider;
//         public TMP_Text Card3EXP_TextOnSlider;
//         public TMP_Text Card4EXP_TextOnSlider;
//         public TMP_Text Card5EXP_TextOnSlider;
//         
//         public TMP_Text Card1Level_Text;
//         public TMP_Text Card2Level_Text;
//         public TMP_Text Card3Level_Text;
//         public TMP_Text Card4Level_Text;
//         public TMP_Text Card5Level_Text;
//
//
//
//         private void Start()
//         {
//             Zone_Text.text = "Zone: " + Zone;
//             
//             _initEnemy();
//             _initCards();
//             
//             enemyCurrentHP = Enemy.Hp;
//             
//             teamCurrentHP = TotalCardStat.Hp;
//
//             UpdateAllTeamStats_UI();
//
//             this.StartCoroutine(BattleUpdate());
//         }
//         
//         
//         private void Update()
//         {
//
//             teamCurrentHP += TotalCardStat.HpRegeneration * Time.deltaTime;
//             if (teamCurrentHP > TotalCardStat.Hp)
//             {
//                 teamCurrentHP = TotalCardStat.Hp;
//             }
//             
//             UpdateTeamSliderHP_UI();
//
//             // bossCurrentHP = Mathf.Lerp(bossCurrentHP, Boss.HP, Time.deltaTime * 5f);
//
//             // teamCurrentHP = Mathf.Lerp(teamCurrentHP, TotalCardStat.HP, Time.deltaTime * 5f);
//             // TeamHP_Slider.value = teamCurrentHP / TotalCardStat.HP;
//
//             // UpdateBossSliderHP_UI();
//             // UpdateAllTeamStats_UI();
//         }
//         
//         
//         private IEnumerator BattleUpdate()
//         {
//             while (true)
//             {
//                 DealDamageToBoss();
//                 DealDamageToTeam();
//
//                 if (enemyCurrentHP <= 0)
//                 {
//                     OnBossDefeated();
//                 }
//
//                 if (teamCurrentHP <= 0)
//                 {
//                     OnEnemyWin();
//                 }
//
//                 yield return new WaitForSeconds(1f);
//             }
//         }
//         
//         
//         private void DealDamageToBoss()
//         {
//             var totalDamage = TotalCardStat.Attack;
//
//             // Critical attack case
//             if (Random.value * 100 < TotalCardStat.Crit)
//             {
//                 totalDamage *= 1 + TotalCardStat.CritDmg / 10f;
//                 enemyCurrentHP -= totalDamage;
//                 if (enemyCurrentHP <= 0)
//                 {
//                     enemyCurrentHP = 0;
//                 }
//                 UpdateEnemySliderHP_UI();
//                 return;
//             }
//
//             
//             enemyCurrentHP -= totalDamage;
//             if (enemyCurrentHP <= 0)
//             {
//                 enemyCurrentHP = 0;
//             }
//             UpdateEnemySliderHP_UI();
//         }
//
//         
//         private void DealDamageToTeam()
//         {
//             var damageToTeam = Zone % 10 == 0 ? Boss.Attack : Enemy.Attack;
//             
//             if (Random.value * 100 < TotalCardStat.Evade)
//             {
//                 return;
//             }
//             
//             if (Random.value * 100 < TotalCardStat.Block)
//             {
//                 damageToTeam *= (1 - TotalCardStat.BlockPower / 100f);
//
//                 teamCurrentHP -= damageToTeam;
//                 if (teamCurrentHP <= 0)
//                 {
//                     teamCurrentHP = 0;
//                     UpdateTeamSliderHP_UI();
//                     return;
//                 }
//             }
//
//             teamCurrentHP -= damageToTeam;
//             if (teamCurrentHP <= 0)
//             {
//                 teamCurrentHP = 0;
//             }
//             UpdateTeamSliderHP_UI();
//         }
//         
//
//         private void OnBossDefeated()
//         {
//             ReceiveExp();
//             
//             if (Random.value * 100 < 50f)
//             {
//                 Debug.Log("Reward granted!");
//             }
//             
//             Zone++;
//             Zone_Text.text = $"Zone: {Zone}";
//
//             if (Zone % 10 == 0)
//             {
//                 _initBoss();
//                 EnemyType_Text.text = "BOSS STATS";
//             }
//             else
//             {
//                 _initEnemy();
//                 EnemyType_Text.text = "UNIT STATS";
//             }
//             
//             
//         }
//         
//         
//         private void OnEnemyWin()
//         {
//             Zone--;
//             if (Zone <= 0)
//             {
//                 Zone = 1;
//             }
//             Zone_Text.text = $"Zone: {Zone}";
//
//             if (Zone % 10 == 0)
//             {
//                 _initBoss();
//                 EnemyType_Text.text = "BOSS STATS";
//             }
//             else
//             { 
//                 _initEnemy();
//                 EnemyType_Text.text = "UNIT STATS";
//             }
//
//
//             teamCurrentHP = TotalCardStat.Hp;
//             UpdateTeamSliderHP_UI();
//         }
//
//         
//         private void ReceiveExp()
//         {
//             if (CardList.Count <= 0)
//             {
//                 return;
//             }
//
//             float experience = Zone % 10 == 0 ? Boss.ExpReward : Enemy.ExpReward;
//             
//             var expPerCard = experience / CardList.Count;
//
//             foreach (var card in CardList)
//             {
//                 card.ExpCurrent += expPerCard;
//                 if (card.ExpCurrent >= card.ExpToNextLevel)
//                 {
//                     while (card.ExpCurrent >= card.ExpToNextLevel)
//                     {
//                         card.Level += 1;
//                         card.ExpCurrent -= card.ExpToNextLevel;
//                         card.ExpToNextLevel = CalculateExpToNextLevel(card);
//                     }
//                     UpdateCardStats(card);
//                     UpdateAllTeamStats_UI();
//                 }
//                 UpdateCardUI(card);
//             }
//         }
//
//         
//         private static float CalculateExpToNextLevel(Card card)
//         {
//             var scale = card.Rarity switch
//             {
//                 Rarity.Common => 1.1f,
//                 Rarity.Rare => 1.2f,
//                 Rarity.Epic => 1.5f,
//                 Rarity.Legendary => 2.5f,
//                 _ => throw new ArgumentException($"Undefined card rarity: {card.Rarity}. Cannot calculate experience")
//             };
//
//             return card.StartBaseExp * Mathf.Pow(card.Level, scale);
//         }
//         
//         
//         private static void UpdateCardStats(Card card)
//         {
//             switch (card.Rarity)
//             {
//                 case Rarity.Common:
//                     card.CurrentHp *= 1.01f;
//                     card.Attack *= 1.025f;
//                     break;
//                 case Rarity.Rare:
//                     card.CurrentHp *= 1.05f;
//                     card.Attack *= 1.050f;
//                     card.Crit += 0.1f;
//                     break;
//                 case Rarity.Epic:
//                     card.CurrentHp *= 1.1f;
//                     card.Attack *= 1.1f;
//                     card.Crit += 0.5f;
//                     card.CritDmg += 10;
//                     break;
//                 case Rarity.Legendary:
//                     card.CurrentHp *= 1.1f;
//                     card.HpRegeneration += 5f;
//                     card.Attack *= 1.1f;
//                     card.Crit += 1f;
//                     card.CritDmg += 25;
//                     card.Block += 1;
//                     card.BlockPower += 2;
//                     break;
//                 default:
//                     throw new ArgumentException($"Undefined card rarity: {card.Rarity}");
//             }
//         }
//         
//         
//         #region UI
//
//         
//         private void UpdateEnemySliderHP_UI()
//         {
//             float unitHp;
//             
//             if (Zone % 10 == 0)
//             {
//                 unitHp = Boss.Hp;
//             }
//             else
//             {
//                 unitHp = Enemy.Hp;
//             }
//             
//             
//             EnemyHP_Slider.value = enemyCurrentHP / unitHp;
//             EnemyHP_TextOnSlider.text = enemyCurrentHP.ToString("F0");
//         }
//
//
//         private void UpdateTeamSliderHP_UI()
//         {
//             TeamHP_Slider.value = teamCurrentHP / TotalCardStat.Hp;
//             TeamHP_TextOnSlider.text = teamCurrentHP.ToString("F0");
//         }
//         
//         
//         private void UpdateAllTeamStats_UI()
//         {
//             TotalCardStat = new TotalCardStatsDto();
//             
//             // foreach (var card in CardList)
//             // {
//             //     TotalCardStat.Hp += card.Hp;
//             //     TotalCardStat.HpRegeneration += card.HpRegeneration;
//             //     TotalCardStat.Attack += card.Attack;
//             //     TotalCardStat.Crit += card.Crit;
//             //     TotalCardStat.CritDmg += card.CritDmg;
//             //     TotalCardStat.Block += card.Block;
//             //     TotalCardStat.BlockPower += card.BlockPower;
//             //     TotalCardStat.Evade += card.Evade;
//             // }
//             //
//             
//             HP_Text.text = $"HP: {TotalCardStat.Hp}";
//             HPs_Text.text = $"HP/s: {TotalCardStat.HpRegeneration:F1}";
//             Attack_Text.text = $"Attack: {TotalCardStat.Attack}";
//             Crit_Text.text = $"Crit: {TotalCardStat.Crit}%";
//             CritDmg_Text.text = $"Crit Dmg: {TotalCardStat.CritDmg}%";
//             Block_Text.text = $"Block: {TotalCardStat.Block}%";
//             BlockPower_Text.text = $"Block Power: {TotalCardStat.BlockPower}%";
//             Evade_Text.text = $"Evade: {TotalCardStat.Evade}";
//         }
//
//
//         private void UpdateEnemyStatAndRewards_UI()
//         {
//             if (Zone % 10 == 0)
//             {
//                 EnemyAttack_Text.text = "Attack: " + Boss.Attack.ToString("F1");
//                 EnemyExpReward_Text.text = "Exp: " + Boss.ExpReward.ToString("F1");
//                 EnemyGoldReward_Text.text = "Gold: " + Boss.GoldReward.ToString("F1");
//             }
//             else
//             {
//                 EnemyAttack_Text.text = "Attack: " + Enemy.Attack.ToString("F1");
//                 EnemyExpReward_Text.text = "Exp: " + Enemy.ExpReward.ToString("F1");
//                 EnemyGoldReward_Text.text = "Gold: " + Enemy.GoldReward.ToString("F1");
//             }
//         }
//         
//         
//         private void UpdateCardUI(Card card)
//         {
//         //     if (card.ExpSlider != null)
//         //     {
//         //         card.ExpSlider.value = card.ExpCurrent / card.ExpToNextLevel;
//         //     }
//         //
//         //     if (card.ExpTextOnSlider != null)
//         //     {
//         //         card.ExpTextOnSlider.text = $"{card.ExpCurrent:F0} / {card.ExpToNextLevel:F0}";
//         //     }
//         //
//         //     if (card.LevelText != null)
//         //     {
//         //         card.LevelText.text = $"lvl {card.Level}";
//         // }
//         }
//         
//         #endregion
//
//
//         #region Init
//
//         private void _initBoss()
//         {
//             if (Boss == null)
//             {
//                 Boss = new Boss
//                 {
//                     Hp = Zone * 1000f,
//                     Attack = Zone * 15f,
//                     ExpReward = Zone * 10,
//                     GoldReward = Zone * 10
//                 };
//             }
//
//             
//             Boss.Hp = Zone * 1000f;
//             Boss.Attack = Zone * 15f;
//             Boss.ExpReward = Zone * 30;
//             Boss.GoldReward = Zone * 30;
//             
//             enemyCurrentHP = Boss.Hp;
//             
//             UpdateEnemyStatAndRewards_UI();
//             UpdateEnemySliderHP_UI();
//         }
//
//
//         private void _initEnemy()
//         {
//             // if (Enemy == null)
//             // {
//             //     Enemy = new EnemyModel
//             //     {
//             //         Hp = Zone * 150f,
//             //         Attack = Zone * 5f,
//             //         ExpReward = (int)(Zone * 15),
//             //         GoldReward = (int)(Zone * 15)
//             //     };
//             // }
//
//             Enemy.Hp = Zone * 150f;
//             Enemy.Attack = Zone * 5f;
//             Enemy.ExpReward = (int)(Zone * 15);
//             Enemy.GoldReward = (int)(Zone * 15);
//             
//             enemyCurrentHP = Enemy.Hp;
//             
//             UpdateEnemyStatAndRewards_UI();
//             UpdateEnemySliderHP_UI();
//         }
//
//
//         private void _initCards()
//         {
//             CardList.Add(new Card
//             {
//                 Id = 1, 
//                 CurrentHp = 100,
//                 Title = "Epic Card",
//                 Level = 1, 
//                 ExpCurrent = 0,
//                 ExpToNextLevel = 500,
//                 StartBaseExp = 500,
//                 HpRegeneration = 0.1f,
//                 Attack = 11, 
//                 Crit = 0.5f,
//                 CritDmg = 0, 
//                 Block = 0, 
//                 BlockPower = 0,
//                 Evade = 0, 
//                 Rarity = Rarity.Rare,
//                 // ExpSlider = Card1Exp_Slider, 
//                 // ExpTextOnSlider = Card1EXP_TextOnSlider,
//                 // LevelText = Card1Level_Text
//             });
//             
//             CardList.Add(new Card 
//             { 
//                 Id = 2, 
//                 CurrentHp = 100, 
//                 Title = "Legendary Card", 
//                 Level = 1, 
//                 ExpCurrent = 0, 
//                 ExpToNextLevel = 1000, 
//                 StartBaseExp = 1000, 
//                 HpRegeneration = 10f, 
//                 Attack = 10, 
//                 Crit = 20, 
//                 CritDmg = 100, 
//                 Block = 25, 
//                 BlockPower = 50, 
//                 Evade = 25, 
//                 Rarity = Rarity.Legendary,
//                 // ExpSlider = Card2Exp_Slider, 
//                 // ExpTextOnSlider = Card2EXP_TextOnSlider,
//                 // LevelText = Card2Level_Text
//             });
//             
//             CardList.Add(new Card 
//             { 
//                 Id = 3, 
//                 CurrentHp = 100, 
//                 Title = "Legendary Card 3", 
//                 Level = 1, 
//                 ExpCurrent = 0, 
//                 ExpToNextLevel = 1000, 
//                 StartBaseExp = 1000, 
//                 HpRegeneration = 0, 
//                 Attack = 0, 
//                 Crit = 0, 
//                 CritDmg = 0, 
//                 Block = 0, 
//                 BlockPower = 0, 
//                 Evade = 0, 
//                 Rarity = Rarity.Legendary,
//                 // ExpSlider = Card3Exp_Slider, 
//                 // ExpTextOnSlider = Card3EXP_TextOnSlider,
//                 // LevelText = Card3Level_Text
//             });
//             
//             CardList.Add(new Card 
//             { 
//                 Id = 4, 
//                 CurrentHp = 100, 
//                 Title = "Rare Card 4", 
//                 Level = 1, 
//                 ExpCurrent = 0, 
//                 ExpToNextLevel = 100, 
//                 StartBaseExp = 100, 
//                 HpRegeneration = 0, 
//                 Attack = 0, 
//                 Crit = 0, 
//                 CritDmg = 0, 
//                 Block = 0, 
//                 BlockPower = 0, 
//                 Evade = 0, 
//                 Rarity = Rarity.Rare,
//                 // ExpSlider = Card4Exp_Slider, 
//                 // ExpTextOnSlider = Card4EXP_TextOnSlider,
//                 // LevelText = Card4Level_Text
//             });
//
//             
//             CardList.Add(new Card 
//             { 
//                 Id = 5, 
//                 CurrentHp = 100, 
//                 Title = "Rare Card 5", 
//                 Level = 1, 
//                 ExpCurrent = 0, 
//                 ExpToNextLevel = 100, 
//                 StartBaseExp = 100, 
//                 HpRegeneration = 0, 
//                 Attack = 0, 
//                 Crit = 0, 
//                 CritDmg = 0, 
//                 Block = 0, 
//                 BlockPower = 0, 
//                 Evade = 0, 
//                 Rarity = Rarity.Rare,
//                 // ExpSlider = Card5Exp_Slider, 
//                 // ExpTextOnSlider = Card5EXP_TextOnSlider,
//                 // LevelText = Card5Level_Text
//             });
//
//
//             // foreach (var card in CardList)
//             // {
//             //     TotalCardStat.Hp += card.Hp;
//             //     TotalCardStat.HpRegeneration += card.HpRegeneration;
//             //     TotalCardStat.Attack += card.Attack;
//             //     TotalCardStat.Crit += card.Crit;
//             //     TotalCardStat.CritDmg += card.CritDmg;
//             //     TotalCardStat.Block += card.Block;
//             //     TotalCardStat.BlockPower += card.BlockPower;
//             //     TotalCardStat.Evade += card.Evade;
//             // }
//         }
//
//         #endregion
//     }
// }
