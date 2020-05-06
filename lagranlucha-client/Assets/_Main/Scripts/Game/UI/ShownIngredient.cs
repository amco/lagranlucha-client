using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using CFLFramework.Utilities.Extensions;
using DG.Tweening;

namespace LaGranLucha.Game
{
    public class ShownIngredient : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Image icon = null;
        [SerializeField] private Image background = null;
        [SerializeField] private CanvasGroup canvasGroup = null;

        private Ingredient ingredient = null;

        private bool showing = false;

        #endregion

        #region PROPERTIES

        public Ingredient Ingredient { get; private set; } = null;

        #endregion

        #region BEHAVIORS

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

            canvasGroup.Hide();
            transform.DOScale(Vector3.zero, 0.5f);
            showing = false;
        }

        public void Show()
        {
            if (showing)
                return;

            canvasGroup.Show();
            transform.DOScale(Vector3.one, 0.5f);
            showing = true;
        }

        #endregion
    }
}
