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
        [SerializeField] protected string baseUrl = "http://localhost:8080/api/";

        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Performs an API GET request.
        /// </summary>
        protected async Task<string> GetAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(baseUrl + url);
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
            try
            {
                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(baseUrl + url, content);
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
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync(baseUrl + url);
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
            try
            {
                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(baseUrl + url, content);
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
