using Assets.Scripts.Core.API;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.API;
using Assets.Scripts.Core.Events.XR;
using Assets.Scripts.Core.Parameters;
using Assets.Scripts.Core.Storing;
using Assets.Scripts.Core.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.UI;
using XRInteractorEvent = Assets.Scripts.Core.XR.XRInteractorEvent;

namespace Assets.Scripts.Core.Players
{
    public class Player : MonoBehaviour
    {
        [Header("Storage")]
        public StorageManager StorageManager;

        [Header("Parameters")]
        public Parameter Parameter;

        [NonSerialized]
        public PlayerData PlayerData;

        private string storagePath;
        private string jsonName;

        private EventBinding<InitXRHandEvent> _initXRHandBinding;
        private EventBinding<OnAPIResponseEvent> _onAPIResponseBinding;

        public void Awake()
        {
            if(StorageManager == null)
            {
                throw new Exception("Storage Manager is not set");
            }

            if (Parameter == null)
            {
                throw new Exception("Parameter is not set");
            }

            _initXRHandBinding = new EventBinding<InitXRHandEvent>(OnInitXRHand);
            _onAPIResponseBinding = new EventBinding<OnAPIResponseEvent>(OnAPIResponse);

            EventBus<InitXRHandEvent>.Register(_initXRHandBinding);
            EventBus<OnAPIResponseEvent>.Register(_onAPIResponseBinding);
        }

        public void Start()
        {
            GetFilePath();
            LoadData();
            SaveData();
        }

        #region Player Data

        /// <summary>
        /// Check and get the file path of the player data
        /// </summary>
        private void GetFilePath()
        {
            storagePath = Application.persistentDataPath + "/Player/";

            if (!System.IO.Directory.Exists(storagePath))
                System.IO.Directory.CreateDirectory(storagePath);

            jsonName = storagePath + "player.json";

            if (!System.IO.File.Exists(jsonName))
                System.IO.File.Create(jsonName).Close();
        }

        /// <summary>
        /// Load data of player
        /// </summary>
        private void LoadData()
        {
            PlayerData = StorageManager.Get<PlayerData>(jsonName);
        }

        /// <summary>
        /// Save the data of the player
        /// </summary>
        private void SaveData()
        {
            if (PlayerData == null)
            {
                PlayerData = new PlayerData();
                PlayerData.PlayerId = Guid.NewGuid().ToString();
                PlayerData.PlayerName = "Unknown";
            }

            StorageManager.Store<PlayerData>(jsonName, PlayerData);
        }

        #endregion

        #region API Response

        /// <summary>
        /// Manage the API response
        /// </summary>
        /// <param name="e"></param>
        private void OnAPIResponse(OnAPIResponseEvent e)
        {
            if (e.RequestType == APIRequestType.GET)
            {
                if (e.URL.Contains("players"))
                {
                    PlayerData player = StorageManager.GetFromString<PlayerData>(e.JSON);

                    if (player != null)
                    {
                        PlayerData = player;
                        ChangeName(PlayerData.PlayerName);
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// Change the name of the player
        /// </summary>
        /// <param name="name"></param>
        public void ChangeName(string name)
        {
            storagePath = Application.persistentDataPath + "/Player/";

            if (!System.IO.Directory.Exists(storagePath))
                System.IO.Directory.CreateDirectory(storagePath);

            jsonName = storagePath + "player.json";

            if (!System.IO.File.Exists(jsonName))
                System.IO.File.Create(jsonName).Close();

            PlayerData.PlayerName = name;

            StorageManager.Store<PlayerData>(jsonName, PlayerData) ;
        }

        #region Init XR Events

        /// <summary>
        /// Place all the event to register interaction with the primary hand
        /// </summary>
        public void InitPrimaryHandXREvent()
        {

        }

        /// <summary>
        /// Place all the event to register interaction with the secondary hand
        /// </summary>
        public void InitSecondaryHandXREvent()
        {
            // Register the toggle parameter event
            EventBus<RegisterXRInputEvent>.Raise(new RegisterXRInputEvent()
            {
                Hand = XRHand.Secondary,
                InputAction = Parameter.ToggleParam,
                InputType = XRInputType.Parameters
            });

        }

        #endregion

        #region XR Events

        /// <summary>
        /// Function to initialize the XR Hand
        /// </summary>
        /// <param name="e"></param>
        private void OnInitXRHand(InitXRHandEvent e)
        {
            InitPrimaryHandXREvent();
            InitSecondaryHandXREvent();
        }

        #endregion

    }
}
