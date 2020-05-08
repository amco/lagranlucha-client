using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Zenject;

namespace LaGranLucha.Game
{
    public class IngredientButton : MonoBehaviour
    {
        #region FIELDS

        private const float CompletelyFilledAmount = 1f;

        [Inject] private OrderManager orderManager = null;

        [SerializeField] private Ingredient ingredient = null;
        [SerializeField] private Image icon = null;
        [SerializeField] private Button button = null;
        [SerializeField] private Image cooldown = null;

        private int step = 0;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            PrepareIngredient();
        }

        private void PrepareIngredient()
        {
            button.interactable = false;
            cooldown.DOFillAmount(CompletelyFilledAmount, ingredient.IngredientSteps[step].PreparationTime).From().SetEase(Ease.Linear).OnComplete(IngredientPrepared);
        }

        private void IngredientPrepared()
        {
            if (++step >= ingredient.IngredientSteps.Length)
                SendIngredient();

            SetGraphics();
            button.interactable = true;
        }

        private void SendIngredient()
        {
            orderManager.SendIngredient(ingredient);
            step = default(int);
        }

        private void OnValidate()
        {
            SetGraphics();
        }

        private void SetGraphics()
        {
            if (icon == null || ingredient == null)
                return;

            icon.sprite = ingredient.IngredientSteps[step].Icon;
        }

        #endregion
    }
}
