using System.Collections.Generic;
using UnityEngine;

namespace LaGranLucha.Game
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Order : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "La Gran Lucha/Game/Order";

        [SerializeField] private List<Product> hamburguer = null;
        [SerializeField] private List<Product> fries = null;
        [SerializeField] private List<Product> drink = null;

        #endregion

        #region PROPERTIES

        public List<Product> Hamburguer { get => hamburguer; }
        public List<Product> Fries { get => fries; }
        public List<Product> Drink { get => drink; }

        #endregion
    }
}
