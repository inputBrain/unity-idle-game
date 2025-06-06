using Model.Card;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Tooltip
{
    /// <summary>
    /// Displays detailed information about a card when hovered.
    /// </summary>
    public class CardTooltipView : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text critText;
        [SerializeField] private TMP_Text critDmgText;
        [SerializeField] private TMP_Text blockText;
        [SerializeField] private TMP_Text blockPowerText;
        [SerializeField] private TMP_Text evadeText;

        public void Show(CardModel card, Vector3 position)
        {
            if (card == null) return;

            if (iconImage != null && !string.IsNullOrEmpty(card.IconResourcesPath.Value))
                iconImage.sprite = Resources.Load<Sprite>(card.IconResourcesPath.Value);

            if (titleText != null) titleText.text = card.Title;
            if (levelText != null) levelText.text = $"Lvl: {card.Level.Value}";
            if (rankText != null) rankText.text = $"Rank: {card.Rank.Value}";
            if (hpText != null) hpText.text = $"HP: {(int)card.CurrentHp.Value} / {(int)card.MaxHp.Value}";
            if (attackText != null) attackText.text = $"Atk: {(int)card.Attack.Value}";
            if (critText != null) critText.text = $"Crit: {(int)card.Crit.Value}%";
            if (critDmgText != null) critDmgText.text = $"CritDmg: +{(int)card.CritDmg.Value}%";
            if (blockText != null) blockText.text = $"Block: {(int)card.Block.Value}%";
            if (blockPowerText != null) blockPowerText.text = $"BlockPower: {(int)card.BlockPower.Value}%";
            if (evadeText != null) evadeText.text = $"Evade: {(int)card.Evade.Value}%";

            transform.position = position;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
