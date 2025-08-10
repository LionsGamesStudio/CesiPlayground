using System.Collections.Generic;
using UnityEngine;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A simple client-side utility to manage a list of UI tabs,
    /// showing one and hiding the others.
    /// </summary>
    public class TabsManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> tabs = new List<GameObject>();

        private void Start()
        {
            // By default, show the first tab if it exists.
            if (tabs.Count > 0)
            {
                ChangeTab(0);
            }
        }

        /// <summary>
        /// Deactivates all tabs except the one at the specified index.
        /// To be called from UI Button OnClick() events.
        /// </summary>
        /// <param name="index">The index of the tab to display.</param>
        public void ChangeTab(int index)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i] != null)
                {
                    tabs[i].SetActive(i == index);
                }
            }
        }
    }
}