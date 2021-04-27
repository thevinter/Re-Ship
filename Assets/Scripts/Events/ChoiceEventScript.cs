using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEventScript : MonoBehaviour
{
    public Choice positiveChoice;
    public Choice negativeChoice;
    public StringVariable s;
   
    public void RunPositive() {
        positiveChoice.GetChoice()(s);
    }

    public void RunNegative() {
        negativeChoice.GetChoice()(s);
    }

}
