using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetInfo : MonoBehaviour, IDisplayable
{
    public TextMeshProUGUI text;
    Dictionary<ShipStat, int> stats = new Dictionary<ShipStat, int>();
    Dictionary<ShipStat, int> delta = new Dictionary<ShipStat, int>();
    void Awake() {
        //text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Display(string s) {
        text.text = GenerateInfo();
    }
    void Update() {
        stats = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipManager>().GetStats();
        delta = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipManager>().GetDelta();
    }

    string GenerateInfo() {
        var to_return = "";
        if (stats != null) {
            foreach (ShipStat stat in stats.Keys) {
                var temp = stat + ": " + stats[stat] + " [" + delta[stat] + "]\n";
                to_return += temp;
            }
            
        }
        return to_return;
    }
}
