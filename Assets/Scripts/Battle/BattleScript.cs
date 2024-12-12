using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        private float teamHealthTarget;

        private void Start()
        {
            Zone_Text.text = "Zone: " + Zone;
            
            _initBoss();
            _initCards();

            bossCurrentHP = Boss.HP;
            teamCurrentHP = TotalCardStat.HP;
            teamHealthTarget = TotalCardStat.HP;

            UpdateStatsUI();
            StartCoroutine(BattleUpdate());
        }
        
        
        private void Update()
        {
            // bossCurrentHP = Mathf.Lerp(bossCurrentHP, Boss.HP, Time.deltaTime * 5f);

            // teamCurrentHP = Mathf.Lerp(teamCurrentHP, TotalCardStat.HP, Time.deltaTime * 5f);
            // TeamHP_Slider.value = teamCurrentHP / TotalCardStat.HP;

            UpdateBossUI();
            UpdateStatsUI();
        }
        
        
        private IEnumerator BattleUpdate()
        {
            while (true)
            {
                DealDamageToBoss();
                // DealDamageToTeam();
                // RegenerateTeamHealth();

                if (bossCurrentHP <= 0)
                {
                    OnBossDefeated();
                }

                yield return new WaitForSeconds(0.1f);
                
                Debug.Log("Boss HP: " + Boss.HP);
                Debug.Log("Boss Current hp: " + bossCurrentHP);
               
            }
        }
        
        
        
        private void DealDamageToBoss()
        {
            float totalDamage = TotalCardStat.Attack;

            // if (Random.value * 100 < TotalCardStat.Crit)
            // {
            //     totalDamage *= 1 + TotalCardStat.CritDmg / 100f;
            // }
            
          //  StartCoroutine(LerpValue(bossCurrentHP, bossCurrentHP- totalDamage));
            
           bossCurrentHP -= totalDamage;
        }

        
        private void DealDamageToTeam()
        {
            float damageToTeam = Boss.Attack;

            foreach (var card in CardList)
            {
                float blockedDamage = damageToTeam * (card.BlockPower / 100f);
                damageToTeam -= blockedDamage;

                if (Random.value * 100 < card.Evade)
                {
                    damageToTeam = 0;
                }

                card.HP -= damageToTeam;

                if (card.HP < 0)
                {
                    card.HP = 0;
                }
            }

            TotalCardStat.HP = 0;
            foreach (var card in CardList)
            {
                TotalCardStat.HP += card.HP;
            }
        }

        
        private void RegenerateTeamHealth()
        {
            foreach (var card in CardList)
            {
                if (card.HP > 0)
                {
                    card.HP += (int)card.HPRegeneration;
                    if (card.HP > 100)
                    {
                        card.HP = 100;
                    }
                }
            }

            TotalCardStat.HP = 0;
            foreach (var card in CardList)
            {
                TotalCardStat.HP += card.HP;
            }
        }
        

        private void OnBossDefeated()
        {
            Zone++;
            Zone_Text.text = $"Zone: {Zone}";

            Boss.HP *= 1.2f;
            Boss.Attack *= 1.05f;
            Boss.ExpReward += 5;
            Boss.GoldReward += 2;

            bossCurrentHP = Boss.HP;
            UpdateBossRewardInfo();

            if (Random.value < 0.05f)
            {
                Debug.Log("Reward granted!");
            }
        }

        
        #region UI

        
        private void UpdateBossUI()
        {
            BossHP_Slider.value = bossCurrentHP / Boss.HP;
            BossHP_TextOnSlider.text = bossCurrentHP.ToString("F1");
        }
        
        private void UpdateStatsUI()
        {
            HP_Text.text = $"HP: {TotalCardStat.HP}";
            HPs_Text.text = $"HP/s: {TotalCardStat.HPRegeneration}";
            Attack_Text.text = $"Attack: {TotalCardStat.Attack}";
            Crit_Text.text = $"Crit: {TotalCardStat.Crit}%";
            CritDmg_Text.text = $"Crit Dmg: {TotalCardStat.CritDmg}%";
            Block_Text.text = $"Block: {TotalCardStat.Block}%";
            BlockPower_Text.text = $"Block Power: {TotalCardStat.BlockPower}%";
            Evade_Text.text = $"Evade: {TotalCardStat.Evade}";

            TeamHP_Slider.value = teamCurrentHP / 1000f;
            TeamHP_TextOnSlider.text = TotalCardStat.HP.ToString();
        }
        
        private void UpdateBossRewardInfo()
        {
            BossAttack_Text.text = "Attack: " + Boss.Attack.ToString("F1");
            BossExpReward_Text.text = "Exp: " + Boss.ExpReward.ToString("F1");
            BossGoldReward_Text.text = "Gold: " + Boss.GoldReward.ToString("F1");
        }
        
        
        #endregion

        #region Init

        private void _initBoss()
        {
            Boss = new BossModel()
            {
                HP = 5000f,
                Attack = 10f,
                ExpReward = 10,
                GoldReward = 1
            };
            
            UpdateBossRewardInfo();
            UpdateBossUI();
        }


        private void _initCards()
        {
            CardList.Add(new CardModel() { Id = 1, HP = 1000, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 1.1f, Attack = 1000, Crit = 25, CritDmg = 50, Block = 5, BlockPower = 25, Evade = 5, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 2, HP = 1000, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 1.1f, Attack = 10, Crit = 15, CritDmg = 60, Block = 10, BlockPower = 30, Evade = 10, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 3, HP = 1000, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 1.1f, Attack = 10, Crit = 8, CritDmg = 45, Block = 7, BlockPower = 20, Evade = 7, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 4, HP = 1000, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 1.1f, Attack = 10, Crit = 12, CritDmg = 55, Block = 6, BlockPower = 28, Evade = 6, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 5, HP = 1000, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 1.1f, Attack = 10, Crit = 11, CritDmg = 52, Block = 9, BlockPower = 29, Evade = 8, Rarity = Rarity.Epic });

            foreach (var card in CardList)
            {
                TotalCardStat.HP += card.HP;
                TotalCardStat.HPRegeneration += card.HPRegeneration;
                TotalCardStat.Attack += card.Attack;
                TotalCardStat.Crit += card.Crit;
                TotalCardStat.CritDmg += card.CritDmg;
                TotalCardStat.Block += card.Block;
                TotalCardStat.BlockPower += card.BlockPower;
                TotalCardStat.Evade += card.Evade;
            }
        }

        #endregion
  
    }
}
