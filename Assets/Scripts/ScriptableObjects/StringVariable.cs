using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{
    public string s; 
    
    public string GetValue() {
        return s;
    }

    public void SetValue(string s) {
        this.s = s;
    }
}
