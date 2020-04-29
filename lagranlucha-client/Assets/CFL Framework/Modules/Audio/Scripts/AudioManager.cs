using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;
using CFLFramework.Data;
using CFLFramework.Utilities.Inspector;
using DG.Tweening;

namespace CFLFramework.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region FIELDS

        private const float CooldownForSounds = 0.05f;
        private const string AudioPreferencesKey = "audio_preferences";
        public const float MusicMaxVolume = 0.65f;
        public const float SoundMaxVolume = 1f;
        private const float FadeOutDuration = 2f;

        private static readonly string[] MusicVolumeKeys = { AudioPreferencesKey, "music_volume" };
        private static readonly string[] SoundVolumeKeys = { AudioPreferencesKey, "sound_volume" };
        private static readonly string[] MusicMuteKeys = { AudioPreferencesKey, "music_mute" };
        private static readonly string[] SoundMuteKeys = { AudioPreferencesKey, "sound_mute" };

        [Inject] private DataManager dataManager = null;

        [Header("VOLUMES")]
        [ReadOnly] [SerializeField] private float musicVolume = MusicMaxVolume;
        [ReadOnly] [SerializeField] private float soundVolume = SoundMaxVolume;

        private AudioSource musicChannel = null;
        private AudioSource musicChannelCrossFadeHelper = null;
        private AudioSource soundChannel = null;
        private Dictionary<int, AudioSource> persistentSoundsChannels = new Dictionary<int, AudioSource>();
        private List<AudioClip> currentSoundClips = new List<AudioClip>();
        private int persistentCounter = -1;

        #endregion

        #region PROPERTIES

        public bool MusicMuted { get => musicChannel.mute; }
        public float MusicVolume { get => musicVolume; }

        public bool SoundMuted { get => soundChannel.mute; }
        public float SoundVolume { get => soundVolume; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            musicChannel = gameObject.AddComponent<AudioSource>();
            musicChannelCrossFadeHelper = gameObject.AddComponent<AudioSource>();
            soundChannel = gameObject.AddComponent<AudioSource>();

            LoadVolume();
            LoadMute();
        }

        private void LoadVolume()
        {
            musicChannel.volume = musicVolume = dataManager.GetData<float>(MusicVolumeKeys, MusicMaxVolume);
            soundChannel.volume = soundVolume = dataManager.GetData<float>(SoundVolumeKeys, SoundMaxVolume);
        }

        public void SetMusicVolume(float volume)
        {
            musicChannel.volume = musicVolume = Mathf.Lerp(0, MusicMaxVolume, volume);
            dataManager.SetData(MusicVolumeKeys, musicVolume);
        }

        public void SetSoundVolume(float volume)
        {
            soundChannel.volume = soundVolume = Mathf.Lerp(0, SoundMaxVolume, volume);
            dataManager.SetData(SoundVolumeKeys, soundVolume);
        }

        private void LoadMute()
        {
            musicChannel.mute = dataManager.GetData<bool>(MusicMuteKeys, false);
            soundChannel.mute = dataManager.GetData<bool>(SoundMuteKeys, false);
        }

        public void ToggleMusicMute()
        {
            musicChannel.mute = !musicChannel.mute;
            dataManager.SetData(MusicMuteKeys, musicChannel.mute);
        }

        public void ToggleSoundMute()
        {
            soundChannel.mute = !soundChannel.mute;
            dataManager.SetData(SoundMuteKeys, soundChannel.mute);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            StopMusic();
            musicChannel.clip = clip;
            musicChannel.loop = loop;
            musicChannel.Play();
        }

        public void CrossFadeMusic(AudioClip clip, bool loop = true)
        {
            if (musicChannel.isPlaying)
            {
                musicChannelCrossFadeHelper.clip = musicChannel.clip;
                musicChannelCrossFadeHelper.time = musicChannel.time;
                musicChannelCrossFadeHelper.volume = musicChannel.volume;
                musicChannelCrossFadeHelper.loop = musicChannel.loop;
                musicChannelCrossFadeHelper.Play();
                musicChannelCrossFadeHelper.DOFade(default(int), FadeOutDuration).OnComplete(() => musicChannelCrossFadeHelper.Stop());
            }

            PlayMusic(clip, loop);
            musicChannel.volume = 0;
        }

        public void StopMusic()
        {
            musicChannel.clip = null;
            musicChannel.Stop();
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip == null || currentSoundClips.Contains(clip))
                return;

            soundChannel.PlayOneShot(clip);
            currentSoundClips.Add(clip);

            StartCoroutine(SoundCooldown(clip, CooldownForSounds));
        }

        public IEnumerator SoundCooldown(AudioClip clip, float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            currentSoundClips.Remove(clip);
        }

        public int PlayPersistentSound(AudioClip clip, float duration = 0f)
        {
            AudioSource persistentSoundChannel = gameObject.AddComponent<AudioSource>();
            persistentSoundChannel.volume = soundChannel.volume;
            persistentSoundChannel.mute = soundChannel.mute;
            persistentSoundChannel.loop = true;
            persistentSoundChannel.clip = clip;
            persistentSoundChannel.Play();

            persistentCounter++;
            persistentSoundsChannels.Add(persistentCounter, persistentSoundChannel);

            if (duration > 0)
                StartCoroutine(StopPersistentSoundCoroutine(duration, persistentCounter));

            return persistentCounter;
        }

        public void StopPersistentSound(int persistentSoundId)
        {
            if (!PersistentSoundExists(persistentSoundId))
                return;

            persistentSoundsChannels[persistentSoundId].Stop();
            Destroy(persistentSoundsChannels[persistentSoundId]);
            persistentSoundsChannels.Remove(persistentSoundId);
        }

        private IEnumerator StopPersistentSoundCoroutine(float duration, int id)
        {
            yield return new WaitForSeconds(duration);
            StopPersistentSound(id);
        }

        private bool PersistentSoundExists(int persistentSoundId)
        {
            return persistentSoundsChannels.ContainsKey(persistentSoundId);
        }

        #endregion
    }
}
