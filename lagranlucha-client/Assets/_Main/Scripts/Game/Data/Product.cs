using System.Collections.Generic;
using UnityEngine;

namespace LaGranLucha.Game
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Product : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "La Gran Lucha/Game/Product";

        [SerializeField] private new string name = null;
        [SerializeField] private List<Ingredient> ingredients = null;

        #endregion

        #region PROPERTIES

        public string Name { get => name; }
        public List<Ingredient> Ingredients { get => ingredients; }

        #endregion
    }
}
