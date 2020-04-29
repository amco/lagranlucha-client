using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor;

using Zenject;
using CFLFramework.Data;
using CFLFramework.Time;
using CFLFramework.Utilities.Inspector;

namespace CFLFramework.Lives
{
    public class LivesManager : MonoBehaviour
    {
        #region FIELDS

        private const string ResetToolName = "CFL Framework/Lives/Reset Lives Data";
        private const string LivesBaseKey = "lives_data";
        private const string LivesStoredKey = "lives";
        private const string TimeForMaxLivesKey = "time_for_max_lives";
        private const float DefaultMinutesPerRound = 30;
        private const int DefaultLivesPerRound = 1;
        private const int DefaultMaxLives = 5;

        private static readonly string[] LivesKeys = new string[] { LivesBaseKey, LivesStoredKey };
        private static readonly string[] TimeForNextLiveKeys = new string[] { LivesBaseKey, TimeForMaxLivesKey };

        [Inject] private DataManager dataManager = null;
        [Inject] private TimeManager timeManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private float minutesPerRound = DefaultMinutesPerRound;
        [SerializeField] private int livesPerRound = DefaultLivesPerRound;
        [SerializeField] private int maxLives = DefaultMaxLives;
        [SerializeField] private int maxLivesAbsolute = DefaultMaxLives;
        [SerializeField] private Sprite lifeSprite = null;

        [Header("STATES")]
        [ReadOnly] [SerializeField] private int lives = 0;

        private bool enableLivesTimer = false;
        private TimeSpan timeForMaxLives = default(TimeSpan);
        private TimeSpan timeForNextLife = default(TimeSpan);
        private bool loaded = false;

        #endregion

        #region EVENTS

        public event UnityAction livesLoaded;
        public event UnityAction livesUpdated;
        public event UnityAction endTimer;
        public event UnityAction enableTimer;

        #endregion

        #region PROPERTIES

        private float MillisecondsInRound { get => minutesPerRound * 60 * 1000; }
        public TimeSpan TimeForNextLive { get => timeForNextLife; }
        public int Lives { get => lives; }
        public int MaxLives { get => maxLives; }
        public bool IsLoaded { get => loaded; }
        public bool TimerEnabled { get => enableLivesTimer; }
        public Sprite LifeSprite { get => lifeSprite; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            timeManager.onTimeLoaded += LoadLives;
        }

        private void OnApplicationPause(bool paused)
        {
            if (UnityEngine.Time.realtimeSinceStartup < 1)
                return;

            if (!paused)
            {
                timeManager.onTimeLoaded += LoadLives;
                timeManager.UpdateTime();
            }
        }

        private void Update()
        {
            if (enableLivesTimer)
            {
                if (timeForNextLife.TotalMilliseconds > 0)
                    timeForNextLife = timeForNextLife.Subtract(TimeSpan.FromSeconds(UnityEngine.Time.unscaledDeltaTime));
                else
                    CheckNextLife();
            }
        }

        private void LoadLives()
        {
            timeManager.onTimeLoaded -= LoadLives;
            lives = dataManager.GetData<int>(LivesKeys, default(int));
            timeForMaxLives = TimeSpan.FromTicks(GetCurrentMaxTimeTicks());

            LoadTimeForNextLife();
            livesLoaded?.Invoke();

            if (timeForMaxLives.TotalMilliseconds <= timeManager.GetTimeMilliseconds())
                EndTimer();
            else
                EnableTimer();

            loaded = true;
        }

        private void LoadTimeForNextLife()
        {
            if (lives >= maxLives)
                return;

            float millisecondsLeft = (float)(timeForMaxLives.TotalMilliseconds - timeManager.GetTimeMilliseconds());
            lives = Mathf.Clamp(maxLives - Mathf.CeilToInt(millisecondsLeft / MillisecondsInRound), 0, maxLivesAbsolute);
            timeForNextLife = TimeSpan.FromMilliseconds(Mathf.Clamp(millisecondsLeft % MillisecondsInRound, 0, Mathf.Infinity));
        }

        private long GetCurrentMaxTimeTicks()
        {
            return long.Parse(dataManager.GetData<string>(TimeForNextLiveKeys, timeManager.GetTime().Ticks.ToString()));
        }

        private void CheckNextLife()
        {
            enableLivesTimer = false;
            AddLives(livesPerRound);
            if (lives >= maxLives)
                EndTimer();
            else
                AddRoundTime();
        }

        private void AddRoundTime()
        {
            if (lives >= maxLives)
                return;

            if (timeForNextLife.TotalMilliseconds <= 0)
                timeForNextLife = timeForNextLife.Add(TimeSpan.FromMinutes(minutesPerRound));

            timeForMaxLives = timeForMaxLives.Add(TimeSpan.FromMinutes(minutesPerRound));
            EnableTimer();
            SaveData();
        }

        public bool SpendLife()
        {
            if (lives > 0)
            {
                lives--;
                AddRoundTime();
                EnableTimer();
                livesUpdated?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddLives(int newLives, bool forced = false)
        {
            timeForMaxLives = timeForMaxLives.Add(TimeSpan.FromMinutes(-minutesPerRound * newLives));

            if (forced)
                lives = Mathf.Clamp(lives + newLives, 0, maxLivesAbsolute);
            else if (lives < maxLives)
                lives = Mathf.Clamp(lives + newLives, 0, maxLives);

            SaveData();
            livesUpdated?.Invoke();
            if (lives >= maxLives)
                EndTimer();
        }

        private void EnableTimer()
        {
            enableLivesTimer = true;
            enableTimer?.Invoke();
        }

        private void EndTimer()
        {
            timeForNextLife = TimeSpan.FromSeconds(0);
            timeForMaxLives = TimeSpan.FromTicks(timeManager.GetTimeTicks());
            SaveData();
            endTimer?.Invoke();
        }

        private void SaveData()
        {
            dataManager.SetData(LivesKeys, lives);
            dataManager.SetData(TimeForNextLiveKeys, timeForMaxLives.Ticks.ToString());
        }

        public void Stop()
        {
            loaded = false;
            enableLivesTimer = false;
        }

#if UNITY_EDITOR
        [MenuItem(ResetToolName)]
        private static void ResetData()
        {
            DataManager.ResetKey(LivesKeys, default(int));
            DataManager.ResetKey(TimeForNextLiveKeys, string.Empty);
        }
#endif

        #endregion
    }
}
