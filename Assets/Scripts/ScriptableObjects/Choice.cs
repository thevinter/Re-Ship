using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Choice : ScriptableObject
{
    public delegate void functionToPass(StringVariable s);
    functionToPass handler;

    public void SetChoice(functionToPass method) {
        handler = method;
    }

    //public void SetChoice(functionToPass method, )

    public functionToPass GetChoice() {
        return handler;
    }
}


