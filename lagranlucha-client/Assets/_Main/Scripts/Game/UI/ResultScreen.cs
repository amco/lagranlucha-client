using UnityEngine;

using CFLFramework.Utilities.Extensions;
using Zenject;
using TMPro;
using DG.Tweening;

namespace LaGranLucha.Game
{
    public class ResultScreen : MonoBehaviour
    {
        #region FIELDS

        private const float ShowScoreStepDuration = 1f;

        [Inject] private GameManager gameManager = null;
        [Inject] private OrderManager orderManager = null;

        [SerializeField] private CanvasGroup resultScreen = null;
        [SerializeField] private TMP_Text hamburgerScore = null;
        [SerializeField] private TMP_Text friesScore = null;
        [SerializeField] private TMP_Text drinkScore = null;
        [SerializeField] private TMP_Text totalScore = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            gameManager.onRoundEnd += Show;
        }

        private void OnDestroy()
        {
            gameManager.onRoundEnd -= Show;
        }

        private void Show()
        {
            resultScreen.Show();
            ShowRoundScore();
        }

        private void ShowRoundScore()
        {
            hamburgerScore.text = friesScore.text = drinkScore.text = default(int).ToString();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(IncreaseNumberTween(hamburgerScore, orderManager.ThisRoundHamburgersScore));
            sequence.Append(IncreaseNumberTween(friesScore, orderManager.ThisRoundFriesScore));
            sequence.Append(IncreaseNumberTween(drinkScore, orderManager.ThisRoundDrinksScore));
            sequence.Append(IncreaseNumberTween(totalScore, orderManager.ThisRoundTotalScore));
        }

        private Tween IncreaseNumberTween(TMP_Text text, float amount)
        {
            return DOTween.To(() => default(int), x => text.text = x.ToString(), amount, ShowScoreStepDuration).SetOptions(true);
        }

        #endregion
    }
}
