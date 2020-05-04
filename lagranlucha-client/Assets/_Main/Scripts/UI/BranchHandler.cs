using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Zenject;
using CFLFramework.Time;

using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public class BranchHandler : MonoBehaviour
    {
        #region FIELDS

        private const string TimeFormat = "hh:mm tt";
        private const string Divider = " - ";

        [Inject] private TimeManager timeManager;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI timeText;

        private Branch branch;
        private Button selectButton;

        #endregion

        #region PROPERTIES

        public TimeSpan TimeOfDay { get => timeManager.GetTime().TimeOfDay; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            selectButton = GetComponent<Button>();
        }

        public void Initialize(Branch branch)
        {
            this.branch = branch;
            SetupUI();
        }

        private void SetupUI()
        {
            nameText.text = branch.Name;
            DateTime openTime = new DateTime().Add(branch.OpenAt);
            DateTime closeTime = new DateTime().Add(branch.CloseAt);
            string openAt = openTime.ToString(TimeFormat);
            string closeAt = closeTime.ToString(TimeFormat);
            timeText.text = openAt + Divider + closeAt;
            bool isOpen = TimeSpan.Compare(TimeOfDay, branch.OpenAt) >= 0 && TimeSpan.Compare(branch.CloseAt, TimeOfDay) == 1;
            selectButton.interactable = isOpen;
        }

        #endregion
    }
}
