using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LaGranLucha.Game
{
    public class ShownIngredient : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Image icon = null;
        [SerializeField] private Image background = null;

        private Ingredient ingredient = null;

        #endregion

        #region PROPERTIES

        public Ingredient Ingredient { get; private set; } = null;

        #endregion

        #region BEHAVIORS

        public void SetIngredient(Ingredient ingredient)
        {
            Ingredient = ingredient;
            gameObject.SetActive(Ingredient != null);
            if (Ingredient != null)
                icon.sprite = Ingredient.IngredientSteps.Last().Icon;
        }

        #endregion
    }
}
