#if UNITY_EDITOR

using System;
using System.IO;
using UnityEngine;

namespace CFLFramework.Utilities.Screenshots
{
    public class ScreenshotTaker : MonoBehaviour
    {
        #region FIELDS

        private const string Route = "Screenshots";
        private const string Extension = ".png";
        private const string DayFormat = "yyyy-MM-dd";
        private const string HoursFormat = "HHmmss";

        [SerializeField] private KeyCode takeScreenshotKey = KeyCode.S;

        #endregion

        #region PROPERTIES

        private string Day { get => DateTime.Now.ToString(DayFormat); }
        private string Hour { get => DateTime.Now.ToString(HoursFormat); }
        private string CompleteDirectory { get => Directory.GetCurrentDirectory() + "/" + Route + "/" + Day; }
        private string CompleteFile { get => CompleteDirectory + "/" + Hour + Extension; }

        #endregion

        #region BEHAVIORS

        private void Update()
        {
            if (Input.GetKeyDown(takeScreenshotKey))
                TakeScreenshot();
        }

        private void TakeScreenshot()
        {
            if (!Directory.Exists(CompleteDirectory))
                Directory.CreateDirectory(CompleteDirectory);

            ScreenCapture.CaptureScreenshot(CompleteFile);
        }

        #endregion
    }
}

#endif
