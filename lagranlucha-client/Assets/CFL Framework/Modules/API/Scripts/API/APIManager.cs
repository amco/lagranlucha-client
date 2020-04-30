using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

using Zenject;
using Newtonsoft.Json;
using JsonApiSerializer;
using CFLFramework.Data;
using CFLFramework.Warnings;

namespace CFLFramework.API
{
    public partial class APIManager : MonoBehaviour
    {
        #region FIELDS

        private const string AuthenticationKey = "authentication";
        private const string ConfigurationPath = "API/Configuration";
        private const string NoSecretTokenErrorMessage = "No secret token found, create one or ask your the project lead.";
        private const string NoConfigurationErrorMessage = "No configuration object found, create one.";
        private const string NoSecretTokenFoundMessage = "No secret token found";

        [Inject] private DataManager dataManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("CONFIGURATION")]
        [SerializeField] [Range(default(float), 50)] private float synchronizationInterval = 5;

        private Requester requester = null;
        private Configuration configuration = null;
        private Coroutine synchronizationCoroutine = null;
        private bool synching = false;

        #endregion

        #region EVENTS

        internal event UnityAction<IAuthentication> onAuthTokenUpdate;
        public event UnityAction<WebRequestResponse> onSynchronizationAttempt;
        public event UnityAction<WebRequestResponse> onUnauthorizedConnectionDetected;
        public event UnityAction onAPILoaded;
        public event UnityAction onLogout;

        #endregion

        #region PROPERTIES

        public bool IsLoggedIn { get { return requester != null && requester.Authentication != null; } }
        public int UserId { get { return requester == null ? -1 : requester.Authentication.Id; } }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            onAuthTokenUpdate += SetAuthentication;

            try
            {
                configuration = GetConfiguration();
                requester = new Requester(configuration.Host, GetAuthentication(), configuration.SecretToken, UnauthorizedConnectionDetected);
                onAPILoaded?.Invoke();
            }
            catch (NullReferenceException)
            {
                warningsManager.ShowWarning(NoSecretTokenFoundMessage);
            }

            StartSynchronization();
        }

        private void OnDestroy()
        {
            onAuthTokenUpdate -= SetAuthentication;
        }

        public void StartSynchronization()
        {
            if (synchronizationInterval > default(float))
                synchronizationCoroutine = StartCoroutine(SynchronizationRoutine());
        }

        private IEnumerator SynchronizationRoutine()
        {
            yield return new WaitForSeconds(synchronizationInterval);

            if (dataManager.IsSynchronizationPending && IsLoggedIn)
            {
                synching = true;
                SynchronizeUser(dataManager.TemporalUser, SynchronizationAttempt);
            }

            synchronizationCoroutine = StartCoroutine(SynchronizationRoutine());
        }

        public void StopSynchronization()
        {
            StartCoroutine(StopSynchronizationCoroutine());
        }

        private IEnumerator StopSynchronizationCoroutine()
        {
            yield return new WaitUntil(() => !synching);
            StopCoroutine(synchronizationCoroutine);
        }

        private void SynchronizationAttempt(WebRequestResponse response)
        {
            synching = false;
            onSynchronizationAttempt?.Invoke(response);
        }

        private Configuration GetConfiguration()
        {
            Configuration configuration = Resources.Load<Configuration>(ConfigurationPath);
            if (configuration == null)
                warningsManager.ShowWarning(NoConfigurationErrorMessage);

            return configuration;
        }

        private void ClearToken()
        {
            requester?.ResetAuthentication();
        }

        private Authentication GetAuthentication()
        {
            string authenticationJson = PlayerPrefs.GetString(AuthenticationKey);
            return JsonConvert.DeserializeObject<Authentication>(authenticationJson);
        }

        private void SaveAuthentication(WebRequestResponse webRequestResponse)
        {
            if (webRequestResponse.Succeeded)
            {
                requester.SaveAuthentication(webRequestResponse.Response<Authentication>());
                SaveAuthenticationLocally();
                onAuthTokenUpdate?.Invoke(requester.Authentication);
            }

            StartSynchronization();
        }

        public void CreateAccount(string username, UnityAction<WebRequestResponse> response)
        {
            StopSynchronization();
            RequestContent requestContent = new RequestContent(RequestType.Post, Endpoint.Users);
            UserData user = new UserData(username);
            requestContent.Content = JsonConvert.SerializeObject(user, new JsonApiSerializerSettings());
            requester.SendRequest(this, requestContent, SaveAuthentication + response);
        }

        public void LogOut()
        {
            dataManager.ResetUser();
            requester.ResetAuthentication();
            PlayerPrefs.DeleteKey(AuthenticationKey);
            ClearToken();
            onLogout?.Invoke();
        }

        public void LinkEmail(string email, UnityAction<WebRequestResponse> response)
        {
            if (email == string.Empty)
                ChangeToUserAuthentication();

            UserData user = new UserData(dataManager.TemporalUser, email);
            SynchronizeUser(user, EmailUpdated + response);
        }

        private void EmailUpdated(WebRequestResponse response)
        {
            if (response.Succeeded)
                dataManager.LinkEmail(response.Response<UserData>().Email);
        }

        public void RequestToken(string username, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Post, Endpoint.RecoveryPassword);
            UserData user = new UserData(username);
            requestContent.Content = JsonConvert.SerializeObject(user, new JsonApiSerializerSettings());
            requester.SendRequest(this, requestContent, response);
        }

        public void SendConfirmationToken(string resetPasswordToken, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Put, Endpoint.RecoveryPassword);
            Authentication resetAuthentication = new Authentication();
            resetAuthentication.ResetPasswordToken = resetPasswordToken;
            requestContent.Content = JsonConvert.SerializeObject(resetAuthentication, new JsonApiSerializerSettings());
            requester.SendRequest(this, requestContent, webRequestResponse =>
            {
                SaveAuthentication(webRequestResponse);

                if (webRequestResponse.Succeeded)
                    LoadUserFromServer(response);
                else
                    response?.Invoke(webRequestResponse);
            });
        }

        private void SaveUserData(System.Object userData, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Put, Endpoint.Users);
            requestContent.Content = JsonConvert.SerializeObject(userData, new JsonApiSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
            requester.SendRequest(this, requestContent, response);
        }

        public void SynchronizeUser(UnityAction<WebRequestResponse> response)
        {
            SaveUserData(dataManager.TemporalUser, response);
        }

        internal void SynchronizeUser(UserData user, UnityAction<WebRequestResponse> response = null)
        {
            SaveUserData(user, ResetTemporalUser + response);
        }

        private void ResetTemporalUser(WebRequestResponse response)
        {
            if (response.Succeeded)
                dataManager.ResetTemporalUser();
        }

        public void LoadUserFromServer(UnityAction<WebRequestResponse> response = null)
        {
            LoadUserData(LoadedUserFromServer + response);
        }

        private void LoadedUserFromServer(WebRequestResponse response)
        {
            if (response.Succeeded)
                dataManager.SetUser(response.Response<UserData>());
        }

        internal void LoadUserData(UnityAction<WebRequestResponse> response = null)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.Users + "/" + requester.Authentication.Id);
            requester.SendRequest(this, requestContent, response);
        }

        public void LoadUserData(string userId, UnityAction<WebRequestResponse> response = null)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.Users);
            requestContent.Parameters.Add(PredicateKey.FieldUser, PredicateValue.Username + "," + PredicateValue.Data);
            requestContent.Parameters.Add(PredicateKey.QueryIdEqual, userId);
            requester.SendRequest(this, requestContent, response);
        }

        private void SetAuthentication(IAuthentication authentication)
        {
            dataManager.SetAuthentication(authentication.Id, authentication.Username, authentication.Email);
        }

        internal void UnauthorizedConnectionDetected(WebRequestResponse response)
        {
            onUnauthorizedConnectionDetected?.Invoke(response);
        }

        internal void ChangeToUserAuthentication()
        {
            var currentAuthentication = requester.Authentication;
            requester.SaveAuthentication(new Authentication(currentAuthentication.Id, dataManager.User.Username, null, currentAuthentication.Token)); SaveAuthenticationLocally();
        }

        internal void ChangeToEmailAuthentication()
        {
            var currentAuthentication = requester.Authentication;
            requester.SaveAuthentication(new Authentication(currentAuthentication.Id, null, dataManager.User.Email, currentAuthentication.Token)); SaveAuthenticationLocally();
        }

        private void SaveAuthenticationLocally()
        {
            PlayerPrefs.SetString(AuthenticationKey, JsonConvert.SerializeObject(requester.Authentication));
        }

        #endregion      
    }
}
