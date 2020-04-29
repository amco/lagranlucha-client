using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Audio
{
    [RequireComponent(typeof(Button))]
    public class PlaySoundOnClick : MonoBehaviour
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
            playOnClickButton.onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            audioManager.PlaySound(audioClip);
        }

        #endregion
    }
}
