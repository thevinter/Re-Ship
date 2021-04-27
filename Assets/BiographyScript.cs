using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BiographyScript : MonoBehaviour {
    public RectTransform rec;
    public RectTransform parent;

    private Unit unit;
    public TextMeshProUGUI text;

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Open() {
        rec.anchoredPosition = new Vector2(-(parent.sizeDelta.x + 10), 0);
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start() {

    }

    private void Awake() {
        
        rec.anchoredPosition = new Vector2(-(parent.sizeDelta.x + 10), 0);
    }

    // Update is called once per frame
    void Update() {
        unit = GameObject.FindGameObjectWithTag("Selected")?.GetComponent<Unit>();
        if (unit == null) {
            text.text = "[Select a unit first]";
        }

        else {

            text.text = unit.bio;
        }
    }
}

    
