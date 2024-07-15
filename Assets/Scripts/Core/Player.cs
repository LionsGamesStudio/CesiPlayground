using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.Players;
using Assets.Scripts.Core.Events.UI;
using Assets.Scripts.Core.Events.XR;
using Assets.Scripts.Core.Games;
using Assets.Scripts.Core.Parameters;
using Assets.Scripts.Core.Storing;
using Assets.Scripts.Core.UI;
using Assets.Scripts.Core.XR;
using Assets.Scripts.UI.PlayerUI;
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

namespace Assets.Scripts.Core
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

        private Game _gameToPlay;

        private EventBinding<InitXRHandEvent> _initXRHandBinding;

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

            EventBus<InitXRHandEvent>.Register(_initXRHandBinding);
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
            EventBus<RegisterXRInteractorUIEvent>.Raise(new RegisterXRInteractorUIEvent()
            {
                Hand = XRHand.Primary,
                XRInteractorEvent = XRInteractorEvent.HoverEnter,
                Callback = OnUIPlayerName
            });
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

            EventBus<RegisterXRInteractorUIEvent>.Raise(new RegisterXRInteractorUIEvent()
            {
                Hand = XRHand.Secondary,
                XRInteractorEvent = XRInteractorEvent.HoverEnter,
                Callback = OnUIPlayerName
            });

        }

        #endregion

        #region XR Events

        private void OnUIPlayerName(UIHoverEventArgs args)
        {
            PlayerNameUI input = args.uiObject.GetComponent<PlayerNameUI>();

            Debug.Log("OnUIPlayerName");

            if (input == null)
                input = args.uiObject.GetComponentInParent<PlayerNameUI>();

            if (input == null)
                input = args.uiObject.GetComponentInChildren<PlayerNameUI>();

            if (input != null)
            {
                input.Player = this;
            }
        }

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
