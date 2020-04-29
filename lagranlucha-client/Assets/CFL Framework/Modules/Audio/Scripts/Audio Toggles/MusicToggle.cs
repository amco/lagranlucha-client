namespace CFLFramework.Audio
{
    public class MusicToggle : AudioToggle
    {
        #region BEHAVIORS

        protected override void Start()
        {
            toggle += audioManager.ToggleMusicMute;
            IsMuted += () => audioManager.MusicMuted;

            base.Start();
        }

        #endregion
    }
}
