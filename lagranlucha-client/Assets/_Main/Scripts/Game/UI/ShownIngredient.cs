using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using CFLFramework.Utilities.Extensions;

namespace LaGranLucha.Game
{
    public class ShownIngredient : MonoBehaviour
    {
        #region FIELDS

        private const float ScaleDuration = 0.5f;

        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] private Image icon = null;
        [SerializeField] private Image background = null;
        [SerializeField] private Sprite currentIngredientBackground = null;
        [SerializeField] private Sprite queuedIngredientBackground = null;
        [SerializeField] private CanvasGroup canvasGroup = null;

        private Ingredient ingredient = null;
        private bool showing = false;
        private Vector2 baseSizeDelta = Vector2.zero;

        #endregion

        #region PROPERTIES

        public Ingredient Ingredient { get; private set; } = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            baseSizeDelta = rectTransform.sizeDelta;
        }

        public void SetIngredient(Ingredient ingredient)
        {
            Ingredient = ingredient;
            if (Ingredient != null)
            {
                icon.sprite = Ingredient.IngredientSteps.Last().Icon;
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Hide()
        {
            if (!showing)
                return;

            canvasGroup.Hide(disableOnEnd: false);
            rectTransform.DOSizeDelta(Vector2.zero, ScaleDuration).SetEase(Ease.OutBounce).OnComplete(() => gameObject.SetActive(false));
            showing = false;
        }

        public void Show()
        {
            if (showing)
                return;

            gameObject.SetActive(true);
            canvasGroup.Show();
            rectTransform.DOSizeDelta(baseSizeDelta, ScaleDuration);
            showing = true;
        }

        public void SetAsCurrentIngredient()
        {
            background.sprite = currentIngredientBackground;
        }

        public void SetAsQueuedIngredient()
        {
            background.sprite = queuedIngredientBackground;
        }

        #endregion
    }
}
