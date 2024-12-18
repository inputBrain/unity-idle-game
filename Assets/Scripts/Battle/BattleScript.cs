using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleScript : MonoBehaviour
    {
        public List<CardModel> CardList = new();
        public TMP_Text Zone_Text;
        public int Zone = 1;

        public TotalCardStatsModel TotalCardStat = new();
        public BossModel Boss = new();

        public Slider BossArmor_Slider;
        public TMP_Text BossArmor_TextOnSlider;

        public Slider BossHP_Slider;
        public TMP_Text BossHP_TextOnSlider;

        public Slider TeamHP_Slider;
        public TMP_Text TeamHP_TextOnSlider;

        public TMP_Text HP_Text;
        public TMP_Text HPs_Text;
        public TMP_Text Attack_Text;
        public TMP_Text Crit_Text;
        public TMP_Text CritDmg_Text;
        public TMP_Text Block_Text;
        public TMP_Text BlockPower_Text;
        public TMP_Text Evade_Text;
        
        public TMP_Text BossAttack_Text;
        public TMP_Text BossExpReward_Text;
        public TMP_Text BossGoldReward_Text;

        private float bossCurrentHP;
        private float teamCurrentHP;
        
        
        public Slider Card1Exp_Slider;
        public Slider Card2Exp_Slider;
        public Slider Card3Exp_Slider;
        public Slider Card4Exp_Slider;
        public Slider Card5Exp_Slider;

        public TMP_Text Card1EXP_TextOnSlider;
        public TMP_Text Card2EXP_TextOnSlider;
        public TMP_Text Card3EXP_TextOnSlider;
        public TMP_Text Card4EXP_TextOnSlider;
        public TMP_Text Card5EXP_TextOnSlider;
        
        public TMP_Text Card1Level_Text;
        public TMP_Text Card2Level_Text;
        public TMP_Text Card3Level_Text;
        public TMP_Text Card4Level_Text;
        public TMP_Text Card5Level_Text;
        //
        // private float card1CurrentExp;
        // private float card2CurrentExp;
        // private float card3CurrentExp;
        // private float card4CurrentExp;
        // private float card5CurrentExp;



        private void Start()
        {
            Zone_Text.text = "Zone: " + Zone;
            
            _initBoss();
            _initCards();

            bossCurrentHP = Boss.HP;
            teamCurrentHP = TotalCardStat.HP;

            UpdateAllTeamStats_UI();

            StartCoroutine(BattleUpdate());
        }
        
        
        private void Update()
        {

            teamCurrentHP += TotalCardStat.HPRegeneration * Time.deltaTime;
            if (teamCurrentHP > TotalCardStat.HP)
            {
                teamCurrentHP = TotalCardStat.HP;
            }
            
            UpdateTeamSliderHP_UI();

            // bossCurrentHP = Mathf.Lerp(bossCurrentHP, Boss.HP, Time.deltaTime * 5f);

            // teamCurrentHP = Mathf.Lerp(teamCurrentHP, TotalCardStat.HP, Time.deltaTime * 5f);
            // TeamHP_Slider.value = teamCurrentHP / TotalCardStat.HP;

            // UpdateBossSliderHP_UI();
            // UpdateAllTeamStats_UI();
        }
        
        
        private IEnumerator BattleUpdate()
        {
            while (true)
            {
                DealDamageToBoss();
                DealDamageToTeam();

                if (bossCurrentHP <= 0)
                {
                    OnBossDefeated();
                }

                if (teamCurrentHP <= 0)
                {
                    OnBossWin();
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
        
        
        private void DealDamageToBoss()
        {
            var totalDamage = TotalCardStat.Attack;

            // Critical attack case
            if (Random.value * 100 < TotalCardStat.Crit)
            {
                totalDamage *= 1 + TotalCardStat.CritDmg / 10f;
                bossCurrentHP -= totalDamage;
                if (bossCurrentHP < 0)
                {
                    bossCurrentHP = 0;
                }
                UpdateBossSliderHP_UI();
                return;
            }

            
            bossCurrentHP -= totalDamage;
            if (bossCurrentHP < 0)
            {
                bossCurrentHP = 0;
            }
            UpdateBossSliderHP_UI();
        }

        
        private void DealDamageToTeam()
        {
            var damageToTeam = Boss.Attack;
            
            if (Random.value * 100 < TotalCardStat.Evade)
            {
                return;
            }
            
            if (Random.value * 100 < TotalCardStat.BlockChance)
            {
                damageToTeam *= (1 - TotalCardStat.BlockPower / 100f);

                teamCurrentHP -= damageToTeam;
                if (teamCurrentHP <= 0)
                {
                    teamCurrentHP = 0;
                    UpdateTeamSliderHP_UI();
                    return;
                }
            }

            teamCurrentHP -= damageToTeam;
            if (teamCurrentHP <= 0)
            {
                teamCurrentHP = 0;
            }
            UpdateTeamSliderHP_UI();
        }
        

        private void OnBossDefeated()
        {
            ReceiveExp();
            
            if (Random.value * 100 < 50f)
            {
                Debug.Log("Reward granted!");
            }
            
            Zone++;
            Zone_Text.text = $"Zone: {Zone}";

            Boss.HP *= 1.05f;
            Boss.Attack *= 1.01f;
            Boss.ExpReward += 10;
            Boss.GoldReward += 1;

            bossCurrentHP = Boss.HP;
            UpdateBossStatAndRewards_UI();
            
        }
        
        
        private void OnBossWin()
        {
            Zone--;
            Zone_Text.text = $"Zone: {Zone}";

            Boss.HP /= 1.05f;
            Boss.Attack /= 1.01f;
            Boss.ExpReward -= 10;
            Boss.GoldReward -= 1;

            bossCurrentHP = Boss.HP;
            teamCurrentHP = TotalCardStat.HP;
            UpdateTeamSliderHP_UI();
            UpdateBossSliderHP_UI();
            UpdateBossStatAndRewards_UI();
        }

        
        private void ReceiveExp()
        {
            if (CardList.Count <= 0)
            {
                return;
            }

            var expPerCard = Boss.ExpReward / CardList.Count;

            foreach (var card in CardList)
            {
                card.ExpCurrent += expPerCard;
                if (card.ExpCurrent >= card.ExpToNextLevel)
                {
                    while (card.ExpCurrent >= card.ExpToNextLevel)
                    {
                        card.Level += 1;
                        card.ExpCurrent -= card.ExpToNextLevel;
                        card.ExpToNextLevel = CalculateExpToNextLevel(card);
                    }
                    UpdateCardStats(card);
                    UpdateAllTeamStats_UI();
                }
                UpdateCardUI(card);
            }
        }

        
        private static float CalculateExpToNextLevel(CardModel card)
        {
            var scale = card.Rarity switch
            {
                Rarity.Common => 1.1f,
                Rarity.Rare => 1.2f,
                Rarity.Epic => 1.5f,
                Rarity.Legendary => 2.5f,
                _ => throw new ArgumentException($"Undefined card rarity: {card.Rarity}. Cannot calculate experience")
            };

            return card.StartBaseExp * Mathf.Pow(card.Level, scale);
        }
        
        
        private static void UpdateCardStats(CardModel card)
        {
            switch (card.Rarity)
            {
                case Rarity.Common:
                    card.HP *= 1.01f;
                    card.Attack *= 1.025f;
                    break;
                case Rarity.Rare:
                    card.HP *= 1.05f;
                    card.Attack *= 1.050f;
                    card.Crit += 0.1f;
                    break;
                case Rarity.Epic:
                    card.HP *= 1.1f;
                    card.Attack *= 1.1f;
                    card.Crit += 0.5f;
                    card.CritDmg += 10;
                    break;
                case Rarity.Legendary:
                    card.HP *= 1.1f;
                    card.HPRegeneration += 5f;
                    card.Attack *= 1.1f;
                    card.Crit += 1f;
                    card.CritDmg += 25;
                    card.Block += 1;
                    card.BlockPower += 2;
                    break;
                default:
                    throw new ArgumentException($"Undefined card rarity: {card.Rarity}");
            }
        }
        
        
        #region UI

        
        private void UpdateBossSliderHP_UI()
        {
            BossHP_Slider.value = bossCurrentHP / Boss.HP;
            BossHP_TextOnSlider.text = bossCurrentHP.ToString("F0");
        }


        private void UpdateTeamSliderHP_UI()
        {
            TeamHP_Slider.value = teamCurrentHP / TotalCardStat.HP;
            TeamHP_TextOnSlider.text = teamCurrentHP.ToString("F0");
        }
        
        
        private void UpdateAllTeamStats_UI()
        {
            TotalCardStat = new TotalCardStatsModel();
            
            foreach (var card in CardList)
            {
                TotalCardStat.HP += card.HP;
                TotalCardStat.HPRegeneration += card.HPRegeneration;
                TotalCardStat.Attack += card.Attack;
                TotalCardStat.Crit += card.Crit;
                TotalCardStat.CritDmg += card.CritDmg;
                TotalCardStat.BlockChance += card.Block;
                TotalCardStat.BlockPower += card.BlockPower;
                TotalCardStat.Evade += card.Evade;
            }
            
            
            HP_Text.text = $"HP: {TotalCardStat.HP}";
            HPs_Text.text = $"HP/s: {TotalCardStat.HPRegeneration:F1}";
            Attack_Text.text = $"Attack: {TotalCardStat.Attack}";
            Crit_Text.text = $"Crit: {TotalCardStat.Crit}%";
            CritDmg_Text.text = $"Crit Dmg: {TotalCardStat.CritDmg}%";
            Block_Text.text = $"Block: {TotalCardStat.BlockChance}%";
            BlockPower_Text.text = $"Block Power: {TotalCardStat.BlockPower}%";
            Evade_Text.text = $"Evade: {TotalCardStat.Evade}";
        }
        
        private void UpdateBossStatAndRewards_UI()
        {
            BossAttack_Text.text = "Attack: " + Boss.Attack.ToString("F1");
            BossExpReward_Text.text = "Exp: " + Boss.ExpReward.ToString("F1");
            BossGoldReward_Text.text = "Gold: " + Boss.GoldReward.ToString("F1");
        }
        
        
        private void UpdateCardUI(CardModel card)
        {
            if (card.ExpSlider != null)
            {
                card.ExpSlider.value = card.ExpCurrent / card.ExpToNextLevel;
            }
    
            if (card.ExpTextOnSlider != null)
            {
                card.ExpTextOnSlider.text = $"{card.ExpCurrent:F0} / {card.ExpToNextLevel:F0}";
            }
    
            if (card.LevelText != null)
            {
                card.LevelText.text = $"lvl: {card.Level}";
            }
        }
        
        #endregion

        #region Init

        private void _initBoss()
        {
            Boss = new BossModel
            {
                HP = 250f,
                Attack = 10f,
                ExpReward = 50,
                GoldReward = 1
            };
            
            UpdateBossStatAndRewards_UI();
            UpdateBossSliderHP_UI();
        }


        private void _initCards()
        {
            CardList.Add(new CardModel
            {
                Id = 1, 
                HP = 100,
                Title = "Epic Card",
                Level = 1, 
                ExpCurrent = 0,
                ExpToNextLevel = 500,
                StartBaseExp = 500,
                HPRegeneration = 0.1f,
                Attack = 11, 
                Crit = 0.5f,
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0,
                Evade = 0, 
                Rarity = Rarity.Rare,
                ExpSlider = Card1Exp_Slider, 
                ExpTextOnSlider = Card1EXP_TextOnSlider,
                LevelText = Card1Level_Text
            });
            
            CardList.Add(new CardModel 
            { 
                Id = 2, 
                HP = 100, 
                Title = "Legendary Card", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 1000, 
                StartBaseExp = 1000, 
                HPRegeneration = 10f, 
                Attack = 10, 
                Crit = 20, 
                CritDmg = 100, 
                Block = 25, 
                BlockPower = 50, 
                Evade = 25, 
                Rarity = Rarity.Legendary,
                ExpSlider = Card2Exp_Slider, 
                ExpTextOnSlider = Card2EXP_TextOnSlider,
                LevelText = Card2Level_Text
            });
            
            CardList.Add(new CardModel 
            { 
                Id = 3, 
                HP = 100, 
                Title = "Legendary Card 3", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 1000, 
                StartBaseExp = 1000, 
                HPRegeneration = 0, 
                Attack = 0, 
                Crit = 0, 
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0, 
                Evade = 0, 
                Rarity = Rarity.Legendary,
                ExpSlider = Card3Exp_Slider, 
                ExpTextOnSlider = Card3EXP_TextOnSlider,
                LevelText = Card3Level_Text
            });
            
            CardList.Add(new CardModel 
            { 
                Id = 4, 
                HP = 100, 
                Title = "Rare Card 4", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 100, 
                StartBaseExp = 100, 
                HPRegeneration = 0, 
                Attack = 0, 
                Crit = 0, 
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0, 
                Evade = 0, 
                Rarity = Rarity.Rare,
                ExpSlider = Card4Exp_Slider, 
                ExpTextOnSlider = Card4EXP_TextOnSlider,
                LevelText = Card4Level_Text
            });

            
            CardList.Add(new CardModel 
            { 
                Id = 5, 
                HP = 100, 
                Title = "Rare Card 5", 
                Level = 1, 
                ExpCurrent = 0, 
                ExpToNextLevel = 100, 
                StartBaseExp = 100, 
                HPRegeneration = 0, 
                Attack = 0, 
                Crit = 0, 
                CritDmg = 0, 
                Block = 0, 
                BlockPower = 0, 
                Evade = 0, 
                Rarity = Rarity.Rare,
                ExpSlider = Card5Exp_Slider, 
                ExpTextOnSlider = Card5EXP_TextOnSlider,
                LevelText = Card5Level_Text
            });


            foreach (var card in CardList)
            {
                TotalCardStat.HP += card.HP;
                TotalCardStat.HPRegeneration += card.HPRegeneration;
                TotalCardStat.Attack += card.Attack;
                TotalCardStat.Crit += card.Crit;
                TotalCardStat.CritDmg += card.CritDmg;
                TotalCardStat.BlockChance += card.Block;
                TotalCardStat.BlockPower += card.BlockPower;
                TotalCardStat.Evade += card.Evade;
            }
        }

        #endregion
    }
}
