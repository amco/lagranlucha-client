using UnityEngine;

using DG.Tweening;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private OrderManager orderManager = null;

        [SerializeField] private Transform hamburguerContainer = null;
        [SerializeField] private Transform friesContainer = null;
        [SerializeField] private Transform drinkContainer = null;
        [SerializeField] private GameIngredient gameIngredientPrefab = null;

        private RectTransform rectTransform = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            orderManager.onHamburguerIngredientSubmit += SendHamburguerIngredient;
            orderManager.onFriesIngredientSubmit += SendFriesIngredient;
            orderManager.onDrinkIngredientSubmit += SendDrinkIngredient;
            orderManager.onOrderCompleted += OrderCompleted;
            orderManager.onHamburguerReset += Resethamburguer;
        }

        private void OrderCompleted()
        {
            rectTransform.DOAnchorPosX(Screen.width, 0.5f).SetDelay(1f).SetEase(Ease.InBack).OnComplete(CleanOrder);
        }

        private void Resethamburguer()
        {
            foreach (Transform child in hamburguerContainer)
                GameObject.Destroy(child.gameObject);
        }

        private void CleanOrder()
        {
            foreach (Transform child in hamburguerContainer)
                GameObject.Destroy(child.gameObject);

            foreach (Transform child in friesContainer)
                GameObject.Destroy(child.gameObject);

            foreach (Transform child in drinkContainer)
                GameObject.Destroy(child.gameObject);

            rectTransform.anchoredPosition = Vector2.zero;
        }

        private void SendHamburguerIngredient(Ingredient ingredient)
        {
            NewIngredient(ingredient, hamburguerContainer);
        }

        private void SendFriesIngredient(Ingredient ingredient)
        {
            NewIngredient(ingredient, friesContainer);
        }

        private void SendDrinkIngredient(Ingredient ingredient)
        {
            NewIngredient(ingredient, drinkContainer);
        }

        private void NewIngredient(Ingredient ingredient, Transform parent)
        {
            Instantiate(gameIngredientPrefab, parent).SetIngredient(ingredient);
        }

        #endregion
    }
}
