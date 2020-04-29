using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;

namespace CFLFramework.DailyRewards
{
    public class ClaimRewardExample : MonoBehaviour
    {
        #region FIELDS

        private const string ClaimedMessage = "Reward {0} claimed.";
        private const string ReadyMessage = "Reward {0} ready.";

        [Inject] private DailyRewardsManager dailyRewardsManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button claimRewardButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            dailyRewardsManager.onClaimedReward += RewardClaimed;
            dailyRewardsManager.onRewardReady += RewardReady;
            claimRewardButton.onClick.AddListener(dailyRewardsManager.ClaimReward);
        }

        private void OnDestroy()
        {
            dailyRewardsManager.onClaimedReward -= RewardClaimed;
            dailyRewardsManager.onRewardReady -= RewardReady;
        }

        private void RewardReady(int round)
        {
            warningsManager.ShowWarning(string.Format(ReadyMessage, round));
        }

        private void RewardClaimed(int round)
        {
            warningsManager.ShowWarning(string.Format(ClaimedMessage, round));
        }

        #endregion
    }
}
