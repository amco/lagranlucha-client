using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.DailyRewards
{
    public class RewardTimerExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private DailyRewardsManager dailyRewardsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Text rewardTimerText = null;

        private bool enableTimer = false;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            dailyRewardsManager.onModuleLoaded += StartTimer;
        }

        private void StartTimer()
        {
            enableTimer = true;
            rewardTimerText.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!enableTimer)
                return;

            rewardTimerText.text = dailyRewardsManager.GetRemainingTimeText();
        }

        #endregion
    }
}
