using Assets.Scripts.Core.API;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.API;
using Assets.Scripts.Core.Players;
using Assets.Scripts.Core.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.API
{
    [CreateAssetMenu(fileName = "APIRequester", menuName = "API/Players", order = 1)]
    public class PlayersRequests : APIRequester
    {
        [SerializeField] private string _tableName = "players";

        #region API Requests

        /// <summary>
        /// Get all players
        /// </summary>
        /// <param name="data"></param>
        public async void GetPlayers(Blackboard data)
        {
            string url = "/" + _tableName;

            string response = await Get(url);

            // No answer
            if (response == null)
            {
                return;
            }

            EventBus<OnAPIResponseEvent>.Raise(new OnAPIResponseEvent()
            {
                RequestType = APIRequestType.GET,
                URL = url,
                JSON = response
            });
        }

        /// <summary>
        /// Get a player by its id
        /// </summary>
        /// <param name="data"></param>
        public async void GetPlayerById(Blackboard data)
        {
            int? id = data.Get<int?>("id");

            if (id == null)
            {
                return;
            }

            string url = "/" + _tableName + "/" + id;

            string response = await Get(url);

            // No answer
            if (response == null)
            {
                return;
            }

            EventBus<OnAPIResponseEvent>.Raise(new OnAPIResponseEvent()
            {
                RequestType = APIRequestType.GET,
                URL = url,
                JSON = response
            });
        }

        /// <summary>
        /// Get a player by its pseudo and password
        /// </summary>
        /// <param name="data"></param>
        public async void GetPlayer(Blackboard data)
        {
            string pseudo = data.Get<string>("pseudo");
            string password = data.Get<string>("password");

            Debug.Log(pseudo + " " + password);

            if (pseudo == null || password == null)
            {
                return;
            }

            //password = HashPassword(password, _salt);

            Debug.Log(password);

            string url = "/" + _tableName + "/" + pseudo;

            string response = await Get(url);

            // No answer
            if (response == null)
            {
                return;
            }

            // TODO : Check if the password is correct

            List<PlayerData> datas = _storageManager.GetFromString<List<PlayerData>>(response);
            string jsonPlayer = "";
            foreach (PlayerData item in datas)
            {
                if (item.PlayerName == pseudo && item.PlayerPassword == password)
                {
                    jsonPlayer = _storageManager.GetFromObject<PlayerData>(item);
                    break;
                }
            }

            response = jsonPlayer;

            EventBus<OnAPIResponseEvent>.Raise(new OnAPIResponseEvent()
            {
                RequestType = APIRequestType.GET,
                URL = url,
                JSON = response
            });
        }

        /// <summary>
        /// Create a player
        /// </summary>
        /// <param name="data"></param>
        public async void CreatePlayer(Blackboard data)
        {
            string pseudo = data.Get<string>("pseudo");
            string password = data.Get<string>("password");

            if (pseudo == null || password == null)
            {
                return;
            }

            // Check if the pseudo is already taken
            string urlCheck = "/" + _tableName + "/" + pseudo;

            string responseCheck = await Get(urlCheck);

            // No answer
            if (responseCheck == null)
            {
                return;
            }

            // If the pseudo is already taken
            if (responseCheck != "[]")
            {
                return;
            }

            // Hash the password
            password = HashPassword(password, _salt);

            // Create the player
            string json = "{\"pseudo\":\"" + pseudo + "\",\"password\":\"" + password + "\"}";
            string url = "/" + _tableName;

            Debug.Log(url);

            string response = await Post(url, json);

            // No answer
            if (response == null)
            {
                return;
            }

            EventBus<OnAPIResponseEvent>.Raise(new OnAPIResponseEvent()
            {
                RequestType = APIRequestType.POST,
                URL = url,
                JSON = response
            });
        }

        /// <summary>
        /// Update a player
        /// </summary>
        /// <param name="data"></param>
        public async void UpdatePlayer(Blackboard data)
        {
            int? id = data.Get<int?>("id");
            string pseudo = data.Get<string>("pseudo");
            string password = data.Get<string>("password");

            if(pseudo == null || password == null || id == null)
            {
                return;
            }

            password = HashPassword(password, _salt);

            string json = "{\"pseudo\":\"" + pseudo + "\",\"password\":\"" + password + "\"}";
            string url = "/" + _tableName + "/" + id;

            string response = await Put(url, json);

            // No answer
            if (response == null)
            {
                return;
            }

            EventBus<OnAPIResponseEvent>.Raise(new OnAPIResponseEvent()
            {
                RequestType = APIRequestType.PUT,
                URL = url,
                JSON = response
            });
        }

        /// <summary>
        /// Delete a player
        /// </summary>
        /// <param name="data"></param>
        public async void DeletePlayer(Blackboard data)
        {
            int? id = data.Get<int?>("id");

            if (id == null)
            {
                return;
            }

            string url = "/" + _tableName + "/" + id;

            string response = await Delete(url);

            // No answer
            if (response == null)
            {
                return;
            }

            EventBus<OnAPIResponseEvent>.Raise(new OnAPIResponseEvent()
            {
                RequestType = APIRequestType.DELETE,
                URL = url,
                JSON = response
            });
        }

        #endregion

        /// <summary>
        /// Hashes the password with the salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, passwordWithSaltBytes, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hash = sha256.ComputeHash(passwordWithSaltBytes);

                // Concatenate the salt and the hash for storage
                byte[] hashWithSalt = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, hashWithSalt, salt.Length, hash.Length);

                return Convert.ToBase64String(hashWithSalt);
            }
        }
    }
}
