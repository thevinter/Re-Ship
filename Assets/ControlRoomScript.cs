using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CREvent {
    wrong,
    broken,
    shortcut,
    creature,
    malfunction,
    noise
}

public class ControlRoomScript : MonoBehaviour, IShip {
    public Image FireImage { get => fireImage; set => throw new System.NotImplementedException(); }
    public Image fireImage;
    public IDictionary<ShipStat, int> stats;
    public ItemSlot Slot { get => slot; }
    public ItemSlot slot;
    public bool isBroken = false;
    public bool hasUnit;
    int repaired = 0;
    CREvent[] events = (CREvent[])System.Enum.GetValues(typeof(CREvent));

    bool canEvent = false;
    public GameEvent infoEvent;
    public GameEvent choiceEvent;
    public Choice positiveChoice;
    public Choice negativeChoice;
    public StringVariable infoText;
    public ShipManager ship;

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
        else if(canEvent) CheckForRandomEvent();
        return stats;
    }
    void CheckForRandomEvent() {
        int result = Random.Range(0, 101);
        int treshold = Slot.activeUnit?.unit_type == UnitType.Cook ? 3 : 3;
        print("Random control" + result);
        if (result < treshold) {
            var selected_event = events[Random.Range(0, events.Length)];
            switch (selected_event) {
                case CREvent.wrong:
                    if (Slot.activeUnit == null) WrongEvent();
                    break;
                case CREvent.broken:
                    BrokenEvent();
                    break;
                case CREvent.shortcut:
                    if (Slot.activeUnit != null) ShortcutEvent();
                    break;
                case CREvent.creature:
                    CreatureEvent();
                    break;

                case CREvent.noise:
                    NoiseEvent();
                    break;
            }
        }
    }

    void WrongEvent() {
        int x = Random.Range(10, 100);
        infoText.SetValue(System.String.Format("No one was in the control room so the ship went off course.\n[-{0} depth]", x));
        ship.depth -= x;
        infoEvent.Raise();
    }

    void BrokenEvent() {
        int x = Random.Range(0,20) + 5;
        if (Slot.activeUnit == null) {
            infoText.SetValue(System.String.Format("A loose panel broke in the control room but no one was there to fix it\n[-15 Energy]"));
            stats[ShipStat.Energy] -= 15;
            infoEvent.Raise();
        }
        else {
            
            infoText.SetValue(System.String.Format("A loose panel broke in the control room. Do you want to send {0} to fix it?", slot.activeUnit.unit_name));
            positiveChoice.SetChoice(
                            (infoText) => {
                                var y = Random.Range(1, 20) + Slot.activeUnit.GetStats()[UnitStat.engineering];
                                if (y > x) {
                                    infoText.SetValue(System.String.Format("{0} managed to fix the problem thanks to his engineering skills", y));
                                    Slot.activeUnit.GetStats()[UnitStat.xp] += 30;

                                }
                                else {
                                    infoText.SetValue(System.String.Format("Ouch, the ship got even more damaged\n[-20 heat]"));
                                    stats[ShipStat.Heat] -= 10;
                                }
                            });
            negativeChoice.SetChoice((infoText) => {
                infoText.SetValue("You decided not to risk it.\n[-10 heat]");
                stats[ShipStat.Heat] -= 10;
            });
            choiceEvent.Raise();
        }

    }

    void ShortcutEvent() {
        int x = Random.Range(10, 100);
        infoText.SetValue(System.String.Format("{1} discovered a shortcut in the rocks.\n[-{0} depth]", x, Slot.activeUnit.unit_name));
        ship.depth += x;
        infoEvent.Raise();
    }

    void CreatureEvent() {

    }

    void NoiseEvent() {
        int x = Random.Range(10, 20);

        infoText.SetValue(System.String.Format("Strange noises are coming from the control room.\n[-{0} happiness]", x));
        stats[ShipStat.Happiness] -= x;
        infoEvent.Raise();
    }
        // Start is called before the first frame update
        void Start()
    {
        StartCoroutine(Allow());
        slot = GetComponent<ItemSlot>();
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 }
        };
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //print(Slot);
        fireImage.gameObject.SetActive(isBroken);
        hasUnit = Slot.activeUnit != null;
    }
}
