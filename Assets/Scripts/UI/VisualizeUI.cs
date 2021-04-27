using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualizeUI : MonoBehaviour
{
    
    public SetData data;
    public SetInfo info;
    public GameObject lvlup_text;

    public void Select() {
        Unit selection = GameObject.FindGameObjectWithTag("Selected").GetComponent<Unit>();
        data.Display(System.String.Format("Name: {0} {2}\nJob: {1}",selection.unit_name, selection.unit_type, selection.unit_surname));
        lvlup_text.SetActive(selection.GetStats()[UnitStat.skillpoints] > 0);

    }
    private void Awake() {
        data.Display("Vital Data:\nSelect a unit first.");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            var temp = GameObject.FindGameObjectWithTag("Selected");
            if (temp != null) {
                data.Display("General info");
                temp.tag = "Untagged";
            }
        }
        info.Display("");
    }
}
