using UnityEngine;

namespace CFLFramework.Audio
{
    public class MusicSlider : AudioSlider
    {
        #region PROPERTIES

        private float CurrentMusicVolumeValue { get => Mathf.InverseLerp(0, AudioManager.MusicMaxVolume, audioManager.MusicVolume) * audioSlider.maxValue; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            isMuted = IsMusicMute;
            newValueSelected = SetMusicVolume;
        }

        protected override void Start()
        {
            UpdateSliderPosition(CurrentMusicVolumeValue);
            base.Start();
        }

        private void SetMusicVolume(float newValue)
        {
            audioManager.SetMusicVolume(newValue);
        }

        private bool IsMusicMute()
        {
            return audioManager.MusicVolume == 0;
        }

        #endregion
    }
}
