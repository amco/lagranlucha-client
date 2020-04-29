using System;
using UnityEngine;
using UnityEditor;

using Zenject;

using CFLFramework.Utilities.Inspector;
using CFLFramework.Time;
using CFLFramework.Data;

using UnityTime = UnityEngine.Time;

namespace CFLFramework.DailyRewards
{
    public class DailyRewardsManager : MonoBehaviour
    {
        #region FIELDS

        private const string DailyRewardsBaseKey = "daily_rewards";
        private const string ResetToolName = "CFL Framework/Daily Rewards/Reset Daily Rewards";
        private const string TimeFormat = "HH:mm:ss";
        private const string DateFormat = "dd/MM/yyyy";
        public const int DefaultCurrentDay = 1;

        private static readonly string[] NextRewardDateKeys = new string[] { DailyRewardsBaseKey, "next_reward_date" };
        private static readonly string[] CurrentRoundKeys = new string[] { DailyRewardsBaseKey, "current_round" };
        private static readonly string[] FirstRewardClaimedKeys = new string[] { DailyRewardsBaseKey, "first_reward_claimed" };

        [Inject] private TimeManager timeManager = null;
        [Inject] private DataManager dataManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string remainingTimeFormat = "{0:D2}:{1:D2}:{2:D2}";
        [SerializeField] private int rewardRounds = 5;
        [SerializeField] private bool giveFirstRewardInstantly = true;

        [Header("TIMES")]
        [SerializeField] [Range(0, 23)] private int hours = 0;
        [SerializeField] [Range(0, 59)] private int minutes = 0;
        [SerializeField] [Range(0, 59)] private int seconds = 0;

        [Header("STATES")]
        [ReadOnly] [SerializeField] private bool timerComplete = false;

        private TimeSpan remainingTime = default(TimeSpan);
        private TimeSpan rewardTime = default(TimeSpan);
        private float progress = 1.0f;
        private bool loaded = false;

        #endregion

        #region EVENTS

        public event Action<int> onRewardReady;
        public event Action<int> onClaimedReward;
        public event Action<TimeSpan> onCalculatedNextDailyTime;
        public event Action onModuleLoaded;

        #endregion

        #region PROPERTIES

        private string NextRewardDate { get; set; }
        private int CurrentRound { get; set; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            timeManager.onTimeLoaded += LoadDailyData;
            timeManager.onTimeLoaded += CheckNextRewardTime;
            CalculateRewardTime();
        }

        private void OnApplicationPause(bool paused)
        {
            if (!paused && loaded)
            {
                timeManager.onTimeLoaded += CheckNextRewardTime;
                timeManager.UpdateTime();
            }
        }

        private void Update()
        {
            if (!loaded || timerComplete)
                return;

            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(UnityTime.deltaTime));
            SetProgress();

            if (progress <= 0)
            {
                timerComplete = true;
                NextRewardReadyToClaim();
            }
        }

        private void LoadDailyData()
        {
            timeManager.onTimeLoaded -= LoadDailyData;
            if (giveFirstRewardInstantly && !dataManager.GetData<bool>(FirstRewardClaimedKeys, false))
            {
                NextRewardDate = TimeSpan.FromTicks(timeManager.GetTimeTicks()).ToString();
                dataManager.SetData<bool>(FirstRewardClaimedKeys, true);
            }
            else
            {
                NextRewardDate = dataManager.GetData<string>(NextRewardDateKeys, string.Empty);
            }

            CurrentRound = dataManager.GetData<int>(CurrentRoundKeys, DefaultCurrentDay);
        }

        private void SaveDailyData()
        {
            dataManager.SetData(NextRewardDateKeys, NextRewardDate);
            dataManager.SetData(CurrentRoundKeys, CurrentRound);
        }

        private void CalculateRewardTime()
        {
            rewardTime = TimeSpan.Parse(hours + ":" + minutes + ":" + seconds);
        }

        private void CheckNextRewardTime()
        {
            timeManager.onTimeLoaded -= CheckNextRewardTime;

            if (string.IsNullOrEmpty(NextRewardDate))
            {
                SetNewRewardTime();
            }
            else
            {
                TimeSpan nextRewardTime = TimeSpan.Parse(NextRewardDate);
                TimeSpan currentTime = TimeSpan.FromTicks(timeManager.GetTimeTicks());
                if (nextRewardTime < currentTime)
                    NextRewardReadyToClaim();
                else
                    remainingTime = nextRewardTime - currentTime;
            }

            loaded = true;
            onModuleLoaded?.Invoke();
        }

        private void SetNewRewardTime()
        {
            TimeSpan currentTime = TimeSpan.FromTicks(timeManager.GetTimeTicks());
            TimeSpan nextRewardTime = currentTime + rewardTime;
            remainingTime = nextRewardTime - currentTime;
            NextRewardDate = nextRewardTime.ToString();
            SaveDailyData();
            SetProgress();
            timerComplete = false;
            onCalculatedNextDailyTime?.Invoke(nextRewardTime);
        }

        private void NextRewardReadyToClaim()
        {
            timerComplete = true;
            onRewardReady?.Invoke(CurrentRound);
        }

        public void ClaimReward()
        {
            if (!timerComplete)
                return;

            onClaimedReward?.Invoke(CurrentRound);
            NextRewardDate = "";
            CurrentRound++;
            if (CurrentRound > rewardRounds)
                CurrentRound = DailyRewardsManager.DefaultCurrentDay;

            SaveDailyData();
            timeManager.onTimeLoaded += CheckNextRewardTime;
            timeManager.UpdateTime();
        }

        private void SetProgress()
        {
            progress = Mathf.InverseLerp(0, rewardTime.Ticks, remainingTime.Ticks);
        }

        public TimeSpan GetRemainingTime()
        {
            return remainingTime;
        }

        public string GetRemainingTimeText()
        {
            return string.Format(remainingTimeFormat, remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
        }

        public double GetProgress()
        {
            return progress;
        }

#if UNITY_EDITOR
        [MenuItem(ResetToolName)]
#endif
        private static void ResetData()
        {
            DataManager.ResetKey(NextRewardDateKeys, string.Empty);
            DataManager.ResetKey(CurrentRoundKeys, DefaultCurrentDay);
            DataManager.ResetKey(FirstRewardClaimedKeys, false);
        }

        #endregion
    }
}
