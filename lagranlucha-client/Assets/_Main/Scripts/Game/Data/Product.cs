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
        [SerializeField] private int points = 0;
        [SerializeField] private int delayedPoints = 0;
        [SerializeField] private float estimatedTime = 0;

        #endregion

        #region PROPERTIES

        public string Name { get => name; }
        public List<Ingredient> Ingredients { get => ingredients; }
        public int Points { get => points; }
        public int DelayedPoints { get => delayedPoints; }
        public float EstimatedTime { get => estimatedTime; }

        #endregion
    }
}
