using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsManager : MonoBehaviour
{
    public List<GameObject> Tabs = new List<GameObject>();

    public void Start()
    {
        ChangeTab(0);
    }

    /// <summary>
    /// Change the tab to display
    /// </summary>
    /// <param name="index"></param>
    public void ChangeTab(int index)
    {
        foreach (GameObject t in Tabs)
        {
            t.SetActive(false);
        }

        Tabs[index].SetActive(true);
    }
}
