using System.Collections.Generic;
using UnityEngine;

namespace LaGranLucha.Game
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Product : MonoBehaviour
    {
        #region FIELDS

        private const string MenuName = "La Gran Lucha/Game/Product";

        [SerializeField] private List<Ingredient> ingredients = null;

        #endregion

        #region PROPERTIES

        public List<Ingredient> Ingredients { get => ingredients; }

        #endregion
    }
}
