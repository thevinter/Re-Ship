using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoEventScript : MonoBehaviour
{

    public GameObject infoPanel;
    public GameObject choicePanel;

    public void ShowInfo()
    {
        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void ShowChoice() {
        choicePanel.SetActive(!choicePanel.activeSelf);
    }

        
    public void ChangeToInfo() {
        print("changing");
        ShowChoice();
        ShowInfo();
    }
        

}
