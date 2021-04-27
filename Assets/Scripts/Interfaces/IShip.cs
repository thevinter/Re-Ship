using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IShip 
{
    Image FireImage { get; set; }
    ItemSlot Slot { get; }
    IDictionary<ShipStat, int> Tick();
    void ResetStats();
}

