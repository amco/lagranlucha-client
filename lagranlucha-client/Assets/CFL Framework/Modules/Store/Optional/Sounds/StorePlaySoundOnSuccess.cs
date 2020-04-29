using UnityEngine;

using CFLFramework.Audio;
using Zenject;

namespace CFLFramework.Store
{
    [RequireComponent(typeof(StoreReward))]
    public class StorePlaySoundOnSuccess : MonoBehaviour
    {
        #region FIELDS

        [Inject] private AudioManager audioManager = null;

        [SerializeField] private AudioClip prizeObtainedSound = null;

        private StoreReward storeReward = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            storeReward = GetComponent<StoreReward>();
            storeReward.onPurchaseCompleted += PlaySound;
        }

        private void OnDestroy()
        {
            storeReward.onPurchaseCompleted -= PlaySound;
        }

        private void PlaySound()
        {
            audioManager.PlaySound(prizeObtainedSound);
        }

        #endregion
    }
}
