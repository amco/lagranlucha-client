using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;
using System.IO;

using Newtonsoft.Json;

namespace CFLFramework.Levels
{
    public class LevelsManager : MonoBehaviour
    {
        #region FIELDS

        private const string LevelsFolder = "/Levels/";
        private const string LevelsResourceFolder = "Levels";

        private List<Level> levels = new List<Level>();

        #endregion

        #region EVENTS

        public event Action<Level> onLevelSaved;
        public event Action<List<Level>> onLoadedLevels;

        #endregion

        #region PROPERTIES

        public string PersistentPath { get => Application.persistentDataPath + LevelsFolder; }

        #endregion

        #region BEHAVIORS

        private IEnumerator Start()
        {
            yield return LoadLevels();
        }

        private IEnumerator LoadLevels()
        {
            yield return LoadPersistentPathLevels();
            LoadResourcesLevels();
            levels = levels.OrderBy((level) => level.Id).ToList();
            onLoadedLevels?.Invoke(levels);
        }

        private IEnumerator LoadPersistentPathLevels()
        {
            if (!Directory.Exists(PersistentPath))
                yield break;

            DirectoryInfo directoryInfo = new DirectoryInfo(PersistentPath);
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                UnityWebRequest dataRequest = UnityWebRequest.Get(file.FullName);
                yield return dataRequest.SendWebRequest();
                levels.Add(JsonConvert.DeserializeObject<Level>(dataRequest.downloadHandler.text));
            }
        }

        private void LoadResourcesLevels()
        {
            TextAsset[] levelsData = Resources.LoadAll<TextAsset>(LevelsResourceFolder);
            foreach (TextAsset levelData in levelsData)
                levels.Add(JsonConvert.DeserializeObject<Level>(levelData.text));
        }

        public void SaveLevel(Level level, string fileName)
        {
            try
            {
                level.FileName = fileName;
                string data = JsonConvert.SerializeObject(level);
                if (!Directory.Exists(PersistentPath))
                    Directory.CreateDirectory(PersistentPath);

                File.WriteAllText(PersistentPath + fileName, data);
                onLevelSaved?.Invoke(level);
            }
            catch (InvalidOperationException)
            {
                throw new LevelNotSavedException();
            }
        }

        public void EraseLevel(Level level)
        {
            EraseLevel(level.FileName);
        }

        public void EraseLevel(string fileName)
        {
            string fullPath = PersistentPath + fileName;
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            else
                throw new LevelNotFoundException();
        }

        public Level GetLevel(string name)
        {
            try
            {
                return levels.First((level) => level.Name == name);
            }
            catch (InvalidOperationException)
            {
                throw new LevelNotFoundException();
            }
        }

        public Level[] GetLevels(string tag)
        {
            Level[] foundLevels = levels.Where((level) => level.Tag == tag).ToArray();
            if (foundLevels.Length == 0)
                throw new LevelNotFoundException();

            return foundLevels;
        }

        public void RefreshLevels()
        {
            levels.Clear();
            StartCoroutine(LoadLevels());
        }

        #endregion
    }
}
