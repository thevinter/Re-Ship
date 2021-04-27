using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public List<SideMenu> window_list;
    public SideMenu stat_panel;
   
    public void OpenOne(string s) {
        foreach (SideMenu m in window_list) {
            if (m.window_name.Equals(s)) m.Activate(true);
            else m.Activate(false);
        }
    }
    // Update is called once per frame
    public void CloseAll()
    {
        foreach (SideMenu m in window_list) {
         
            m.Activate(false);
        }
    }

    public void OpenStatPanel() {
        stat_panel.gameObject.SetActive(true);
    }
}
