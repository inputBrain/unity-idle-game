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

        public BossModel Boss = new ();

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

        private void Start()
        {
            InitializeBoss();
            InitializeCards();
            UpdateStatsUI();
            StartCoroutine(BattleUpdate());
        }

        private void InitializeBoss()
        {
            Boss = new BossModel()
            {
                Armor = 50f,
                HP = 500f,
                Attack = 10f
            };
            UpdateBossUI();
        }

        private void InitializeCards()
        {
            CardList.Add(new CardModel() { Id = 1, HP = 100,Title ="", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0.1f, Attack = 20, Crit = 10, CritDmg = 50, Block = 5, BlockPower = 25, Evade = 5, Rarity = Rarity.Epic});
            CardList.Add(new CardModel() { Id = 2, HP = 150,Title ="", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0.1f,  Attack = 15, Crit = 15, CritDmg = 60, Block = 10, BlockPower = 30, Evade = 10, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 3, HP = 180,Title ="", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0.1f,  Attack = 18, Crit = 8, CritDmg = 45, Block = 7, BlockPower = 20, Evade = 7, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 4, HP = 220,Title ="", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0.1f,  Attack = 22, Crit = 12, CritDmg = 55, Block = 6, BlockPower = 28, Evade = 6, Rarity = Rarity.Epic });
            CardList.Add(new CardModel() { Id = 5, HP = 210,Title ="", Level = 1, ExpCurrent = 1, ExpToNextLevel = 200, HPRegeneration = 0.1f,  Attack = 21, Crit = 11, CritDmg = 52, Block = 9, BlockPower = 29, Evade = 8, Rarity = Rarity.Epic });

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

        private void UpdateStatsUI()
        {
            HP_Text.text = $"HP: {TotalCardStat.HP}";
            HPs_Text.text = $"HP/s: {TotalCardStat.HPRegeneration}";
            Attack_Text.text = $"Attack: {TotalCardStat.Attack}";
            Crit_Text.text = $"Crit: {TotalCardStat.Crit}%";
            CritDmg_Text.text = $"Crit Dmg: {TotalCardStat.CritDmg}%";
            Block_Text.text = $"Block: {TotalCardStat.Block}%";
            BlockPower_Text.text = $"Block Power: {TotalCardStat.BlockPower}%";
            Evade_Text.text = $"Evade: {TotalCardStat.Evade}%";

            TeamHP_Slider.value = 1f;
            TeamHP_TextOnSlider.text = TotalCardStat.HP.ToString();
        }

        private void UpdateBossUI()
        {
            BossArmor_Slider.value = Boss.Armor / 50f;
            BossArmor_TextOnSlider.text = Boss.Armor.ToString("F1");
            BossHP_Slider.value = Boss.HP / 500f;
            BossHP_TextOnSlider.text = Boss.HP.ToString("F1");
        }

        private IEnumerator BattleUpdate()
        {
            while (true)
            {
                DealDamageToBoss();
                DealDamageToTeam();

                if (Boss.HP <= 0)
                {
                    OnBossDefeated();
                }

                UpdateBossUI();
                UpdateStatsUI();

                yield return new WaitForSeconds(1f);
            }
        }

        private void DealDamageToBoss()
        {
            float totalDamage = TotalCardStat.Attack;


            if (Random.value * 100 < TotalCardStat.Crit)
            {
                totalDamage *= 1 + TotalCardStat.CritDmg / 100;
            }

            if (Boss.Armor > 0)
            {
                Boss.Armor -= totalDamage;
                if (Boss.Armor < 0) Boss.Armor = 0;
            }
            else
            {
                Boss.HP -= totalDamage;
                if (Boss.HP < 0) Boss.HP = 0;
            }
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

            TeamHP_Slider.value = TotalCardStat.HP / 1000f;
        }

        private void OnBossDefeated()
        {
            Zone++;
            Zone_Text.text = $"Zone: {Zone}";

            Boss.HP *= 1.2f;
            Boss.Armor *= 1.5f;
            Boss.Attack *= 1.05f;

            if (Random.value < 0.05f)
            {
                Debug.Log("Reward granted!");
            }

            InitializeBoss();
        }
    }
}
