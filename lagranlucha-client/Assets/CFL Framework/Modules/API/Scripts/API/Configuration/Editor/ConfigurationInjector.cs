#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;
using System.IO;

#endif

namespace CFLFramework.API
{
    public static class ConfigurationInjector
    {
        #region FIELDS

        private const string ConfigurationStoragePath = "Assets/CFL Framework/Modules/API/Resources/API/";
        private const string ConfigurationAssetName = "Configuration.asset";
        private const string ErrorMessage = "Configuration asset could not be created.";
        private const string HostKey = "ApiHost";
        private const string SecretTokenKey = "ApiSecretToken";

        #endregion

        #region BEHAVIORS

#if UNITY_EDITOR

        public static void PreExport()
        {
            try
            {
                string host = Environment.GetEnvironmentVariable(HostKey);
                string secretToken = Environment.GetEnvironmentVariable(SecretTokenKey);

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(secretToken))
                    throw new ConfigurationNotCreatedException();

                CreateConfigurationAsset(host, secretToken);
            }
            catch (ConfigurationNotCreatedException)
            {
                Debug.LogError(ErrorMessage);
            }
        }

        private static void CreateConfigurationAsset(string host, string secretToken)
        {
            Configuration configuration = ScriptableObject.CreateInstance<Configuration>();
            configuration.Initialize(host, secretToken);

            if (!Directory.Exists(ConfigurationStoragePath))
                Directory.CreateDirectory(ConfigurationStoragePath);

            AssetDatabase.CreateAsset(configuration, ConfigurationStoragePath + ConfigurationAssetName);
            AssetDatabase.SaveAssets();
        }

#endif

        #endregion
    }
}
