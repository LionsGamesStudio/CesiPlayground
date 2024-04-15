using Assets.Scripts.Core;
using Assets.Scripts.Core.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.PlayerUI
{
    public class ChangeNamePlayerUI : MonoBehaviour
    {
        public TMP_Text InputText;

        public Player Player;

        public StorageManager StorageManager;

        public void ChangeName()
        {
            string newName = InputText.text;
            if (!string.IsNullOrEmpty(newName) && Player != null)
            {
                Player.ChangeName(newName);
            }
        }
    }
}
