using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatMenu : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Unit unit;
    private Dictionary<UnitStat, int> selectedStats;
    public GameObject lvlup;
    public BiographyScript bio;
    // Start is called before the first frame update
    public void IncrementStat(string s) {
        switch (s) {
            case "Engineering":
                unit.SetStat(UnitStat.engineering, 1);
                break;
            case "Cooking":
                unit.SetStat(UnitStat.cooking, 1);
                break;
            case "Shooting":
                unit.SetStat(UnitStat.combat, 1);
                break;
        }
    }

    public void Close() {
        gameObject.SetActive(false);
        bio.Close();
    }

    // Update is called once per frame
    void Update()
    {
        unit = GameObject.FindGameObjectWithTag("Selected")?.GetComponent<Unit>();
        if (unit == null) {
            text.text = "Select a unit first";
            lvlup.SetActive(false);
        }

        else {
            selectedStats = unit.GetStats();
            var s = System.String.Format("Engineering: {0}\n" +
                                         "Cooking      : {1}\n" +
                                         "Shooting     : {2}\n" +
                                         "Experience   : {3}/{5}\n" +
                                         "Happiness    : {4}\n"+
                                         "Level:       : {6}", selectedStats[UnitStat.engineering], selectedStats[UnitStat.cooking], selectedStats[UnitStat.combat], selectedStats[UnitStat.xp], selectedStats[UnitStat.happiness],selectedStats[UnitStat.level]*100, selectedStats[UnitStat.level]);
            text.text = s;
            if (selectedStats[UnitStat.skillpoints] > 0) {
                lvlup.SetActive(true);
            }
            else {
                lvlup.SetActive(false);
            }
        }

    }
}
