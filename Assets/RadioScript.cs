using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public ClientScript client;
    public TextMeshProUGUI text;
    public TextMeshProUGUI status;
    public string status_text;
    public TextMeshProUGUI received;
    public RectTransform rec;
    public RectTransform parent;



    public void Close() {
        status_text = "";
        gameObject.SetActive(false);
    }
    public void Open() {
        rec.anchoredPosition = new Vector2(0, (parent.sizeDelta.y * 2));
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        status_text = "";
        status.text = status_text;
    }

   
    public void SendText() {
        
        var result = client.Send(text.text);
        if (result) status_text = "Sent!";
        else status_text = "You must wait";
    }

    // Update is called once per frame
    void Update()
    {
        //print(client.received_message.Length);   
        if (client.received_message.Length == 0) {
            received.text = "None";
        }
        else received.text = "UIID: ["+ Random.Range(100,5739).ToString() + "] " + client.received_message;
        status.text = status_text;
    }
}
