using UnityEngine;

namespace LaGranLucha.Game
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Order : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "La Gran Lucha/Game/Order";

        [SerializeField] private Product hambruguer = null;
        [SerializeField] private Product fries = null;
        [SerializeField] private Product drink = null;

        #endregion

        #region PROPERTIES

        public Product Hamburguer { get => hamburguer; }
        public Product Fries { get => fries; }
        public Product Drink { get => drink; }

        #endregion
    }
}
