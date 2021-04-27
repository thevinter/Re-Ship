using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum LoungeEvents {
    broke
}
public class LoungeRoom : MonoBehaviour, IShip {
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

    private LoungeEvents[] events = (LoungeEvents[])System.Enum.GetValues(typeof(LoungeEvents));
    IEnumerator Allow() {
        yield return new WaitForSeconds(10);
        canEvent = true;
    }

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
                
                stats[ShipStat.Happiness] += Random.Range(5,10);
            }
            else {
                stats[ShipStat.Happiness] += 0;
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
                case LoungeEvents.broke:
                    if (Slot.activeUnit != null) BreakEvent();
                    break;
            }
        }
    }

    void BreakEvent() {
        infoText.SetValue(System.String.Format("The sofa was destroyed in the lounge room. It is now unusable"));
        isBroken = true;
        infoEvent.Raise();
    }



    // Start is called before the first frame update
    void Start() {
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 }
        };
        StartCoroutine(Allow());
    }

    // Update is called once per frame
    void Update() {
        fireImage.gameObject.SetActive(isBroken);
    }
}
