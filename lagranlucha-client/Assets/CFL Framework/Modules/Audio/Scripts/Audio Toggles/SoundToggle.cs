namespace CFLFramework.Audio
{
    public class SoundToggle : AudioToggle
    {
        #region BEHAVIORS

        protected override void Start()
        {
            toggle += audioManager.ToggleSoundMute;
            IsMuted += () => audioManager.SoundMuted;

            base.Start();
        }

        #endregion
    }
}
