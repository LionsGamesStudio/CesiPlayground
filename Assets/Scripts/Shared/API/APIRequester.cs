using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace CesiPlayground.Shared.API
{
    /// <summary>
    /// An abstract base class providing helper methods for making HTTP requests.
    /// This can be inherited by both client-side and server-side API services.
    /// </summary>
    public abstract class APIRequester : ScriptableObject
    {
        private string _baseUrl;

        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// This method MUST be called by the service initializer after the service is created.
        /// It provides the service with its correct runtime base URL.
        /// </summary>
        public void Initialize(string baseUrl)
        {
            _baseUrl = baseUrl;
            Debug.Log($"[{this.name}] Initialized with Base URL: {_baseUrl}");
        }

        /// <summary>
        /// Performs an API GET request.
        /// </summary>
        protected async Task<string> GetAsync(string url)
        {
            if (string.IsNullOrEmpty(_baseUrl)) { Debug.LogError("APIRequester not initialized!"); return null; }
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_baseUrl + url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"GET Request failed: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Performs an API POST request.
        /// </summary>
        protected async Task<string> PostAsync(string url, string json)
        {
            if (string.IsNullOrEmpty(_baseUrl)) { Debug.LogError("APIRequester not initialized!"); return null; }
            try
            {
                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_baseUrl + url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"POST Request failed: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Performs an API DELETE request.
        /// </summary>
        protected async Task<string> DeleteAsync(string url)
        {
            if (string.IsNullOrEmpty(_baseUrl)) { Debug.LogError("APIRequester not initialized!"); return null; }
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(_baseUrl + url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"DELETE Request failed: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Performs an API PUT request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected async Task<string> PutAsync(string url, string json)
        {
            if (string.IsNullOrEmpty(_baseUrl)) { Debug.LogError("APIRequester not initialized!"); return null; }
            try
            {
                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(_baseUrl + url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"PUT Request failed: {e.Message}");
                return null;
            }
        }
    }
}
