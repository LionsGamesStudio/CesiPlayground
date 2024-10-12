using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.API;
using Assets.Scripts.Core.Storing;
using System;
using System.Collections;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Core.API
{
    public enum APIRequestType
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public abstract class APIRequester : ScriptableObject
    {
        [SerializeField] private string _baseUrl = "http://localhost:5000/api/";

        private HttpClient _client;

        protected byte[] _salt;
        protected StorageManager _storageManager;

        public void Awake()
        {
            if(_salt == null)
                _salt = GenerateSalt();
        }

        public void SetStorageManager(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        /// <summary>
        /// Generate a salt
        /// </summary>
        /// <returns></returns>
        protected byte[] GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        private void CreateClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }
        }
 
        #region API Requests

        /// <summary>
        /// API GET request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<string> Get(string url)
        {
            try
            {
                CreateClient();
                HttpResponseMessage response = await _client.GetAsync(_baseUrl + url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                if(e.Message.Contains("404"))
                {
                    return "[]";
                }

                Debug.LogError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// API POST request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected async Task<string> Post(string url, string json)
        {
            try
            {
                CreateClient();
                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_baseUrl + url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// API PUT request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected async Task<string> Put(string url, string json)
        {
            try
            {
                CreateClient();
                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(_baseUrl + url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// API DELETE request
        /// </summary>
        /// <param name="url">api url</param>
        /// <returns></returns>
        protected async Task<string> Delete(string url)
        {
            try
            {
                CreateClient();
                HttpResponseMessage response = await _client.DeleteAsync(_baseUrl + url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }

        #endregion
    }
}
