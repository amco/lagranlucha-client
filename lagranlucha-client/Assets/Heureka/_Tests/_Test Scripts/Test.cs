using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using NUnit.Framework;

using Zenject;

namespace Tests
{
    public class Test
    {
        #region FIELDS

        private const float ZenjectResetWaitDuration = 0.2f;
        private const string ZenjectObjectName = "Zenject";
        private const string TestObjectDefaultName = "Test Object";

        private GameObject loadedResource;
        private string loadedSceneName;

        #endregion

        #region TESTS

        [TearDown]
        protected void ResetTest()
        {
            if (loadedResource != null)
                UnityEngine.Object.Destroy(loadedResource);

            if (loadedSceneName != null)
            {
                EditorSceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(loadedSceneName));
                loadedSceneName = null;
            }

            ResetTimeScale();
        }

        #endregion

        #region RESOURCES

        protected void InstantiateScene(string name)
        {
            loadedSceneName = name;
            EditorSceneManager.LoadScene(name, LoadSceneMode.Additive);
        }

        protected T InstantiateResource<T>(string name, bool inject = false) where T : UnityEngine.Object
        {
            if (loadedResource == null)
                CreateEmptyResource();

            GameObject newGameObject = inject ? InstantiateZenjectResource(name) : InstantiateUnityResource(name);

            ParentToBaseResource(newGameObject.transform);
            return newGameObject.GetComponent<T>();
        }

        private void CreateEmptyResource()
        {
            loadedResource = new GameObject(TestObjectDefaultName);
        }

        private GameObject InstantiateZenjectResource(string resourceName)
        {
            return GetZenjectContainer().InstantiatePrefab(Resources.Load(resourceName));
        }

        private GameObject InstantiateUnityResource(string resourceName)
        {
            return PrefabUtility.InstantiatePrefab(Resources.Load(resourceName)) as GameObject;
        }

        protected void InstantiateInjectedBinder<T>(string name) where T : UnityEngine.Object
        {
            GetZenjectContainer().BindInstance(InstantiateZenjectResource(name).GetComponent<T>());
        }

        #endregion

        #region BEHAVIORS

        protected IEnumerator WaitForFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
                yield return null;
        }

        protected IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        protected IEnumerator WaitZenjectReset()
        {
            yield return WaitForSeconds(ZenjectResetWaitDuration);
        }

        protected void SetFastTimeScale()
        {
            Time.timeScale = 3f;
        }

        protected void ResetTimeScale()
        {
            Time.timeScale = 1f;
        }

        protected T[] GetGameObjects<T>() where T : UnityEngine.Object
        {
            T[] objects = Resources.FindObjectsOfTypeAll<T>();
            return (T[])objects;
        }

        protected T GetGameObject<T>() where T : UnityEngine.Object
        {
            T[] objects = GetGameObjects<T>();
            return objects.Length > 0 ? (T)objects[0] : null;
        }

        protected T GetGameObject<T>(string name) where T : UnityEngine.Object
        {
            T[] objects = GetGameObjects<T>();
            for (int i = 0; i < objects.Length; i++)
                if (objects[i].name == name)
                    return (T)objects[i];
            return null;
        }

        protected GameObject GetGameObject(string name)
        {
            return GetGameObject<GameObject>(name);
        }

        private DiContainer GetZenjectContainer()
        {
            if (loadedResource != null)
                return loadedResource.GetComponentInChildren<SceneContext>().Container;

            return GameObject.Find(ZenjectObjectName).GetComponentInChildren<SceneContext>().Container;
        }

        protected T GetDependency<T>()
        {
            return GetZenjectContainer().Resolve<T>();
        }

        protected void ParentToBaseResource(Transform transform)
        {
            transform.SetParent(loadedResource.transform);
        }

        #endregion
    }
}