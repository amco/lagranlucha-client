using UnityEngine;
using UnityEngine.UI;

using CFLFramework.Audio;
using Zenject;

namespace CFLFramework.Utilities.Buttons
{
    public class PressSound : MonoBehaviour
    {
        #region FIELDS

        [Inject] private AudioManager audioManager = null;

        [SerializeField] private AudioClip sound = null;

        private Button button = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Pressed);
        }

        private void OnMouseUpAsButton()
        {
            if (button != null)
                return;

            Pressed();
        }

        private void Pressed()
        {
            audioManager.PlaySound(sound);
        }

        #endregion
    }
}
