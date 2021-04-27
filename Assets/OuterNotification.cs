using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterNotification : MonoBehaviour
{
    public GameObject notification;
    public ClientScript client;
    public bool opened = false;
    private string oldString;

    // Start is called before the first frame update
    void Start() {
        oldString = client.received_message;
        if (oldString != "") opened = false;
    }

    public void CloseNotification() {
        notification.SetActive(false);
        opened = true;
        oldString = client.received_message;
    }

    // Update is called once per frame
    void Update() {

        if (!opened) {
            notification.SetActive(true);
        }
        if (client.received_message != oldString && client.received_message != "") {
            opened = false;
            notification.SetActive(true);
        }
        else {
            notification.SetActive(false);
        }

    }
}
