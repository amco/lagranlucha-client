using UnityEngine;

namespace LaGranLucha.Game
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Ingredient : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "La Gran Lucha/Game/Ingredient";

        [SerializeField] private IngredientStep[] ingredientSteps = null;
        [SerializeField] private Sprite gameImage = null;
        [SerializeField] private bool isHamburguerIngredient = true;

        #endregion

        #region PROPERTIES

        public IngredientStep[] IngredientSteps { get => ingredientSteps; }
        public Sprite GameImage { get => gameImage; }
        public bool IsHamburguerIngredient { get => isHamburguerIngredient; }

        #endregion
    }
}
