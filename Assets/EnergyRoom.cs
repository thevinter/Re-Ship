using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum EnergyEvents {
    broke
}
public class EnergyRoom : MonoBehaviour, IShip {
    public Image FireImage { get => fireImage; set => throw new System.NotImplementedException(); }
    public Image fireImage;
    public ItemSlot Slot => slot;
    public ItemSlot slot;
    IDictionary<ShipStat, int> stats;
    int repaired = 0;


    public GameEvent infoEvent;
    public GameEvent choiceEvent;
    public Choice positiveChoice;
    public Choice negativeChoice;
    public StringVariable infoText;
    public ShipManager ship;
    private bool canEvent;
    public bool isBroken;

    private EnergyEvents[] events = (EnergyEvents[])System.Enum.GetValues(typeof(EnergyEvents));


    public void ResetStats() {
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 }
        };
    }

    public IDictionary<ShipStat, int> Tick() {
        if (isBroken) {
            if (Slot.activeUnit != null) {
                var x = Slot.activeUnit.unit_type == UnitType.Engingeer ? 4 : 3;
                repaired += x + Slot.activeUnit.GetStats()[UnitStat.engineering];
            }
            if (repaired > 20) {
                isBroken = false;
                repaired = 0;
            }
            ResetStats();
        }
        else {
            if (Slot.activeUnit != null) {
                int energy = Slot.activeUnit.unit_type == UnitType.Electrician ? 4 + Slot.activeUnit.GetStats()[UnitStat.engineering] : (2 + Slot.activeUnit.GetStats()[UnitStat.engineering]);
                stats[ShipStat.Energy] += energy;
            }
            else {
                stats[ShipStat.Energy] += 0;
            }
            if (canEvent) CheckForRandomEvent();
        }
        return stats;
    }

    void CheckForRandomEvent() {
        int result = Random.Range(0, 101);
        int treshold = Slot.activeUnit?.unit_type == UnitType.Cook ? 2 : 2;
        if (result < treshold) {
            var selected_event = events[Random.Range(0, events.Length)];
            switch (selected_event) {
                case EnergyEvents.broke:
                    if (Slot.activeUnit != null) BreakEvent();
                    break;
            }
        }
    }

    void BreakEvent() {
        infoText.SetValue(System.String.Format("A valve exploded in the Energy Room"));
        isBroken = true;
        infoEvent.Raise();
    }



    // Start is called before the first frame update
    void Start()
    {
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        print(gameObject.name);
        fireImage.gameObject.SetActive(isBroken);
    }
}
