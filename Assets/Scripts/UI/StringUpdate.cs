using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StringUpdate : MonoBehaviour {
    public StringVariable s;

    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() {
        text.text = s.GetValue();
    }
}
