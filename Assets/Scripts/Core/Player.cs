using Assets.Scripts.Core.Storing;
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

        [Header("Parameters")]
        public InputActionReference ParameterInput;
        public GameObject ParametersWindow;

        [NonSerialized]
        public PlayerData PlayerData;

        private string storagePath;
        private string jsonName;


        public void Start()
        {
            // Charging Player
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

            // Input
            ParameterInput.action.started += ToggleParam;
        }

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

        private void ToggleParam(InputAction.CallbackContext call)
        {
            ParametersWindow.SetActive(!ParametersWindow.activeSelf);
        }

    }
}
