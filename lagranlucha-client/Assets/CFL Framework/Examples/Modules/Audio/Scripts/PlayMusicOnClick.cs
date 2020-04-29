using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Audio
{
    [RequireComponent(typeof(Button))]
    public class PlayMusicOnClick : MonoBehaviour
    {
        #region FIELDS

        [Inject] private AudioManager audioManager = null;

        [Header("CONFIGURATION")]
        [SerializeField] private AudioClip audioClip = null;

        private Button playOnClickButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            playOnClickButton = GetComponent<Button>();
            playOnClickButton.onClick.AddListener(PlayMusic);
        }

        private void PlayMusic()
        {
            playOnClickButton.onClick.RemoveAllListeners();
            playOnClickButton.onClick.AddListener(StopMusic);
            audioManager.PlayMusic(audioClip);
        }

        private void StopMusic()
        {
            playOnClickButton.onClick.RemoveAllListeners();
            playOnClickButton.onClick.AddListener(PlayMusic);
            audioManager.StopMusic();
        }

        #endregion
    }
}
