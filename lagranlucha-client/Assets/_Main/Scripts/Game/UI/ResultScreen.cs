using System.Collections;
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

        [Inject] private GameManager gameManager = null;
        [Inject] private OrderManager orderManager = null;

        [SerializeField] private CanvasGroup resultScreen = null;
        [SerializeField] private TMP_Text hamburguerScore = null;
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
            hamburguerScore.text = friesScore.text = drinkScore.text = default(int).ToString();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => 0, x => hamburguerScore.text = x.ToString(), orderManager.ThisRoundHamburguersScore, 1f).SetOptions(true));
            sequence.Append(DOTween.To(() => 0, x => friesScore.text = x.ToString(), orderManager.ThisRoundFriesScore, 1f).SetOptions(true));
            sequence.Append(DOTween.To(() => 0, x => drinkScore.text = x.ToString(), orderManager.ThisRoundDrinksScore, 1f).SetOptions(true));
            sequence.Append(DOTween.To(() => 0, x => totalScore.text = x.ToString(), orderManager.ThisRoundTotalScore, 1f).SetOptions(true));

            //sequence.OnComplete(onEnd);


        }

        #endregion
    }
}
