using UnityEditor;
using UnityEngine;

namespace CFLFramework.Gifts
{
    [CustomEditor(typeof(GiftsManager))]
    public class GiftsManagerEditor : Editor
    {
        private const string PrintGiftsDataButtonMessage = "Print Gifts";
        private const string UserFormat = "GIFTS:\n{0}";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button(PrintGiftsDataButtonMessage))
                Debug.Log(string.Format(UserFormat, PlayerPrefs.GetString(GiftsManager.GiftsSaveDataKey)));
        }
    }
}
