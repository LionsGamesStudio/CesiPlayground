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

namespace Assets.Scripts.Core
{
    public class Player : MonoBehaviour
    {
        [Header("Storage")]
        public StorageManager StorageManager;

        [Header("XR")]
        public XRGlobalManager XRGlobalManager;

        [Header("Parameters")]
        public Parameter Parameter;

        [NonSerialized]
        public PlayerData PlayerData;



        private string storagePath;
        private string jsonName;

        public void Awake()
        {
            if(StorageManager == null)
            {
                throw new Exception("Storage Manager is not set");
            }

            if (XRGlobalManager == null)
            {
                throw new Exception("XR Global Manager is not set");
            }

            if (Parameter == null)
            {
                throw new Exception("Parameter is not set");
            }
        }

        public void Start()
        {
            #region Charging Player Data

            storagePath = Application.persistentDataPath + "/Player/";

            if(!System.IO.Directory.Exists(storagePath))
                System.IO.Directory.CreateDirectory(storagePath);

            jsonName = storagePath + "player.json";

            if(!System.IO.File.Exists(jsonName))
                System.IO.File.Create(jsonName).Close();

            PlayerData = StorageManager.Get<PlayerData>(jsonName);

            if (PlayerData == null)
            {
                PlayerData = new PlayerData();
                PlayerData.PlayerId = Guid.NewGuid().ToString();
                PlayerData.PlayerName = "Unknown";
                StorageManager.Store<PlayerData>(jsonName, PlayerData);
            }

            #endregion


            // Register the toggle parameter event
            XRGlobalManager.RegisterXRInputEvent(new RegisterXRInputEvent()
            {
                Hand = XRHand.Secondary,
                InputAction = Parameter.ToggleParam,
                InputType = XRInputType.Parameters
            });

        }

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

        

    }
}
