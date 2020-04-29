using UnityEditor;
using UnityEngine;

namespace CFLFramework.Data
{
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor : Editor
    {
        private const string PrintUserDataButtonMessage = "Print User";
        private const string UserFormat = "USER:\n{0}";
        private const string PrintTemporalUserDataButtonMessage = "Print Temporal User";
        private const string TemporalUserFormat = "TEMPORAL USER:\n{0}";
        private const string EraseUsersButtonMessage = "Erase Users";
        private const int Space = 10;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button(PrintUserDataButtonMessage))
                Debug.Log(string.Format(UserFormat, PlayerPrefs.GetString(DataManager.UserDataKey)));

            if (GUILayout.Button(PrintTemporalUserDataButtonMessage))
                Debug.Log(string.Format(TemporalUserFormat, PlayerPrefs.GetString(DataManager.TemporalUserDataKey)));

            GUILayout.Space(Space);

            if (GUILayout.Button(EraseUsersButtonMessage))
                DataManager.DeleteUsers();
        }
    }
}
