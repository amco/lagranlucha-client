using UnityEngine;

namespace CFLFramework.Audio
{
    public class SoundSlider : AudioSlider
    {
        #region PROPERTIES

        private float CurrentSoundVolumeValue { get => Mathf.InverseLerp(0, AudioManager.SoundMaxVolume, audioManager.SoundVolume) * audioSlider.maxValue; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            isMuted = IsSoundMute;
            newValueSelected = SetSoundVolume;
        }

        protected override void Start()
        {
            UpdateSliderPosition(CurrentSoundVolumeValue);
            base.Start();
        }

        private void SetSoundVolume(float newValue)
        {
            audioManager.SetSoundVolume(newValue);
        }

        private bool IsSoundMute()
        {
            return audioManager.SoundVolume == 0;
        }

        #endregion
    }
}
