using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Lives
{
    public class LivesTimerExample : MonoBehaviour
    {
        #region FIELDS

        private const string LivesFormat = "x{0}";
        private const string TimeFormat = "{0:00}:{1:00}";

        [Inject] private LivesManager livesManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Image lifeImage = null;
        [SerializeField] private Text currentLivesText = null;
        [SerializeField] private Text timerText = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            lifeImage.sprite = livesManager.LifeSprite;
            livesManager.enableTimer += AppearTimer;
            livesManager.endTimer += DisappearTimer;
            livesManager.livesLoaded += LoadTimer;
            livesManager.livesUpdated += SetLivesText;
        }

        private void OnDestroy()
        {
            livesManager.enableTimer -= AppearTimer;
            livesManager.endTimer -= DisappearTimer;
            livesManager.livesLoaded -= LoadTimer;
            livesManager.livesUpdated -= SetLivesText;
        }

        private void Update()
        {
            if (!livesManager.TimerEnabled)
                return;

            SetTimerText();
        }

        private void LoadTimer()
        {
            SetLivesText();
        }

        private void AppearTimer()
        {
            timerText.gameObject.SetActive(true);
            SetLivesText();
        }

        private void DisappearTimer()
        {
            timerText.gameObject.SetActive(false);
            SetLivesText();
        }

        private void SetLivesText()
        {
            currentLivesText.gameObject.SetActive(true);
            currentLivesText.text = string.Format(LivesFormat, livesManager.Lives);
        }

        private void SetTimerText()
        {
            timerText.text = string.Format(TimeFormat, livesManager.TimeForNextLive.Minutes, livesManager.TimeForNextLive.Seconds);
        }

        #endregion
    }
}
