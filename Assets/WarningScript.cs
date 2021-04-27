using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningScript : MonoBehaviour
{
    private bool shouldChange = true;
    public TextMeshProUGUI text;
    string[] data_values = { "ATTENZIONE", "ATENŢIE", "WARNING", "ACHTUNG", "ВНИМАНИЕ", "ΠΡΟΣΟΧΗ" };
    int i = 0;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (shouldChange) {
            shouldChange = false;
            StartCoroutine(ChangeWord());
        }
    }

    IEnumerator ChangeWord() {
        text.text = data_values[i++];
        if (i == data_values.Length) i = 0;
        yield return new WaitForSeconds(1);
        shouldChange = true;
        yield return null;
    }
}
