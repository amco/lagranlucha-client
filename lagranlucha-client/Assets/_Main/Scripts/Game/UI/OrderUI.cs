using UnityEngine;

using DG.Tweening;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private OrderManager orderManager = null;

        [SerializeField] private Transform withHamburguerContainer = null;
        [SerializeField] private Transform noHamburguerContainer = null;
        [SerializeField] private Transform onlyOneContainer = null;
        [SerializeField] private GameIngredient gameIngredientPrefab = null;

        private Transform hamburguerContainer = null;
        private Transform friesContainer = null;
        private Transform drinkContainer = null;
        private RectTransform rectTransform = null;
        private Vector2 startPosition = Vector2.zero;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = rectTransform.anchoredPosition;
            orderManager.onHamburguerIngredientSubmit += SendHamburguerIngredient;
            orderManager.onFriesIngredientSubmit += SendFriesIngredient;
            orderManager.onDrinkIngredientSubmit += SendDrinkIngredient;
            orderManager.onOrderCompleted += OrderCompleted;
            orderManager.onHamburguerReset += Resethamburguer;
            orderManager.onNewOrderStart += OrderStart;
        }

        private void OrderStart(Order order)
        {
            if (order.HaveHamburger && (order.HaveDrink || order.HaveFries))
            {
                friesContainer = withHamburguerContainer.GetChild(0);
                drinkContainer = withHamburguerContainer.GetChild(1);
                hamburguerContainer = withHamburguerContainer.GetChild(2);
            }
            else if (order.HaveDrink && order.HaveFries)
            {
                friesContainer = noHamburguerContainer.GetChild(0);
                drinkContainer = noHamburguerContainer.GetChild(1);
                hamburguerContainer = null;
            }
            else
            {
                friesContainer = onlyOneContainer.GetChild(0);
                drinkContainer = onlyOneContainer.GetChild(1);
                hamburguerContainer = onlyOneContainer.GetChild(2);
            }
        }

        private void OrderCompleted()
        {
            rectTransform.DOAnchorPosX(Screen.width, 0.5f).SetDelay(0.9f).SetEase(Ease.InBack).OnComplete(CleanOrder);
        }

        private void Resethamburguer()
        {
            if (hamburguerContainer != null)
                foreach (Transform child in hamburguerContainer)
                    GameObject.Destroy(child.gameObject);
        }

        private void CleanOrder()
        {
            if (hamburguerContainer != null)
                foreach (Transform child in hamburguerContainer)
                    GameObject.Destroy(child.gameObject);

            if (friesContainer != null)
                foreach (Transform child in friesContainer)
                    GameObject.Destroy(child.gameObject);

            if (drinkContainer != null)
                foreach (Transform child in drinkContainer)
                    GameObject.Destroy(child.gameObject);

            rectTransform.anchoredPosition = startPosition;
            rectTransform.DOAnchorPosX(-Screen.width, 0.5f).From().SetEase(Ease.OutBack);
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
