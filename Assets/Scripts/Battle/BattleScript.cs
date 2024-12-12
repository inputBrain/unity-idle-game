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
                // RegenerateTeamHealth();

                if (bossCurrentHP <= 0)
                {
                    OnBossDefeated();
                }

                yield return new WaitForSeconds(1f);
            }
        }
        
        
        private void DealDamageToBoss()
        {
            var totalDamage = TotalCardStat.Attack;

            // Critical attack case
            if (Random.value * 100 < TotalCardStat.Crit)
            {
                totalDamage *= 1 + TotalCardStat.CritDmg / 100f;
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
            Zone++;
            Zone_Text.text = $"Zone: {Zone}";

            Boss.HP *= 1.5f;
            Boss.Attack *= 1.2f;
            Boss.ExpReward += 5;
            Boss.GoldReward += 2;

            bossCurrentHP = Boss.HP;
            UpdateBossStatAndRewards_UI();

            if (Random.value < 0.05f)
            {
                Debug.Log("Reward granted!");
            }
        }

        
        #region UI

        
        private void UpdateBossSliderHP_UI()
        {
            BossHP_Slider.value = bossCurrentHP / Boss.HP;
            BossHP_TextOnSlider.text = bossCurrentHP.ToString("F1");
        }


        private void UpdateTeamSliderHP_UI()
        {
            TeamHP_Slider.value = teamCurrentHP / TotalCardStat.HP;
            TeamHP_TextOnSlider.text = teamCurrentHP.ToString("F1");
        }
        
        private void UpdateAllTeamStats_UI()
        {
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
        
        
        #endregion

        #region Init

        private void _initBoss()
        {
            Boss = new BossModel()
            {
                HP = 250f,
                Attack = 10f,
                ExpReward = 10,
                GoldReward = 1
            };
            
            UpdateBossStatAndRewards_UI();
            UpdateBossSliderHP_UI();
        }


        private void _initCards()
        {
            CardList.Add(new CardModel() { Id = 1, HP = 100, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 5.5f, Attack = 10, Crit = 60, CritDmg = 1000, Block = 25, BlockPower = 50, Evade = 25, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 2, HP = 250, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0, Attack = 0, Crit = 0, CritDmg = 0, Block = 0, BlockPower = 0, Evade = 0, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 3, HP = 100, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0, Attack = 0, Crit = 0, CritDmg = 0, Block = 0, BlockPower = 0, Evade = 0, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 4, HP = 100, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0, Attack = 0, Crit = 0, CritDmg = 0, Block = 0, BlockPower = 0, Evade = 0, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 5, HP = 100, Title = "", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0, Attack = 0, Crit = 0, CritDmg = 0, Block = 0, BlockPower = 0, Evade = 0, Rarity = Rarity.Epic });

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
