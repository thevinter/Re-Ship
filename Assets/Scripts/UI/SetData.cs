using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetData : MonoBehaviour, IDisplayable
{
    public TextMeshProUGUI text;
    void Awake() {
        //text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Display(string s) {
        text.text = s;
    }
}
