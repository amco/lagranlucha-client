using UnityEngine;
using UnityEngine.UI;

namespace LaGranLucha.Game
{
    public class GameIngredient : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Image ingredientImage = null;

        #endregion

        #region BEHAVIORS

        public void SetIngredient(Ingredient ingredient)
        {
            ingredientImage.sprite = ingredient.GameImage;
        }

        #endregion
    }
}
