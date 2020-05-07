using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace LaGranLucha.Game
{
    public class ShownIngredient : MonoBehaviour
    {
        #region FIELDS

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

            canvasGroup.DOFade(0, 0.3f);
            rectTransform.DOSizeDelta(Vector2.zero, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => gameObject.SetActive(false));
            showing = false;
        }

        public void Show()
        {
            if (showing)
                return;

            gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.3f);
            rectTransform.DOSizeDelta(baseSizeDelta, 0.5f);
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
