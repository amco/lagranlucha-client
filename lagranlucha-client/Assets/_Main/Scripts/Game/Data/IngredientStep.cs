using System;
using UnityEngine;

namespace LaGranLucha.Game
{
    [Serializable]
    public class IngredientStep
    {
        #region FIELDS

        [SerializeField] private Sprite icon = null;
        [SerializeField] private float preparationTime = 0.2f;
        [SerializeField] private AudioClip preparationSound = null;

        #endregion

        #region PROPERTIES

        public Sprite Icon { get => icon; }
        public float PreparationTime { get => preparationTime; }
        public AudioClip PreparationSound { get => preparationSound; }

        #endregion
    }
}
