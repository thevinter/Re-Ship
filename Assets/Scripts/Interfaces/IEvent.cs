using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent 
{
    IDictionary<ShipStat, int> Tick();
}
