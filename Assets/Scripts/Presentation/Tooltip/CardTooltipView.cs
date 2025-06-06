using System.Collections;
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
        [Header("UI References")]
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

        [Header("Modal Behaviour")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.25f;

        private Coroutine _fadeRoutine;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup != null)
                canvasGroup.alpha = 0f;

            gameObject.SetActive(false);
        }

        public void Show(CardModel card)
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

            if (_rect != null)
                _rect.anchoredPosition = Vector2.zero;

            gameObject.SetActive(true);

            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(Fade(1f));
        }

        public void Hide()
        {
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(Fade(0f));
        }

        private IEnumerator Fade(float target)
        {
            if (canvasGroup == null)
            {
                gameObject.SetActive(target > 0f);
                yield break;
            }

            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            if (target > 0f)
                gameObject.SetActive(true);

            while (time < fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, target, time / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = target;
            if (target <= 0f)
                gameObject.SetActive(false);
        }
    }
}
