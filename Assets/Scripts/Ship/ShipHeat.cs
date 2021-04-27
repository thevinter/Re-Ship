using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum HeatEvent {
    boiler,
    energy,
    steam,
    wire,
    motor,
    evil
}

public class ShipHeat : MonoBehaviour, IShip
{
    public ItemSlot Slot => GetComponent<ItemSlot>();
    private HeatEvent[] heat_events = (HeatEvent[])System.Enum.GetValues(typeof(HeatEvent));
    public Image FireImage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Image fireImage;
    public bool isBroken;
    Dictionary<ShipStat, int> stats;


    public GameEvent infoEvent;
    public GameEvent choiceEvent;
    public Choice positiveChoice;
    public Choice negativeChoice;
    public StringVariable infoText;
    bool canEvent = false;

    IEnumerator Allow() {
        yield return new WaitForSeconds(10);
        canEvent = true;
    }

    public IDictionary<ShipStat, int> Tick() {
        int heat;
        if (Slot.activeUnit != null) {
            heat = Slot.activeUnit.unit_type == UnitType.Engingeer ?3 +Slot.activeUnit.GetStats()[UnitStat.engineering] : 2+Slot.activeUnit.GetStats()[UnitStat.engineering];

        }
        else heat = 1;
        // CheckEvent();
        stats[ShipStat.Heat] += heat;
        return stats;
    }

    void CheckEvent() {
        int result = Random.Range(0, 101);
        int treshold = Slot.activeUnit?.unit_type == UnitType.Cook ? 2 : 2;
        //print(result);
        if (result < treshold) {
            var selected_event = heat_events[Random.Range(0, heat_events.Length)];
            switch (selected_event) {
                case HeatEvent.boiler:
                    if (Slot.activeUnit != null) BoilerEvent();
                    break;
                case HeatEvent.energy:
                    if (Slot.activeUnit != null) EnergyEvent();
                    break;
                case HeatEvent.steam:
                    SteamEvent();
                    break;
                case HeatEvent.wire:
                    if (Slot.activeUnit != null) WireEvent();
                    break;
                case HeatEvent.motor:
                    MotorEvent();
                    break;  
                case HeatEvent.evil:
                    EvilEvent();
                    break;
            }
        }
    }

    void EvilEvent() {
        if (Slot.activeUnit.noFinger) return;
        infoText.SetValue(System.String.Format("{0} was cutting carrots but a sudden movement sliced {1} middle finger away. ", Slot.activeUnit?.unit_name, Slot.activeUnit.possessive_selection));
        Slot.activeUnit.addToBio(System.String.Format("{0} lost a finger while cutting carrots", Slot.activeUnit?.pronoun));
        Slot.activeUnit.noFinger = true;
        infoEvent.Raise();
    }


    void BoilerEvent() {
        infoText.SetValue("One of your boilers broke. Try to fix it?");

        positiveChoice.SetChoice(
            (infoText) => {
                var x = Random.Range(1, 10);
                infoText.SetValue(System.String.Format("You tried rescuing some food. You managed to scavenge {0} pieces of food but the kitchen is now broken.", x));
                stats[ShipStat.Food] += x;
                isBroken = true;
            });
        negativeChoice.SetChoice((infoText) => {
            infoText.SetValue("You left the fire calm down a bit, then you used extinguishers to put it out. The food in the kitchen got completely destroyed but at least the kitchen is fine.");
        });
        choiceEvent.Raise();
    }

    void WireEvent() {
        infoText.SetValue("Your kitchen caught on fire. Too bad you weren't insured. Do you want to try and rescue some food?");

        positiveChoice.SetChoice(
            (infoText) => {
                var x = Random.Range(1, 10);
                infoText.SetValue(System.String.Format("You tried rescuing some food. You managed to scavenge {0} pieces of food but the kitchen is now broken.", x));
                stats[ShipStat.Food] += x;
                isBroken = true;
            });
        negativeChoice.SetChoice((infoText) => {
            infoText.SetValue("You left the fire calm down a bit, then you used extinguishers to put it out. The food in the kitchen got completely destroyed but at least the kitchen is fine.");
        });
        choiceEvent.Raise();
    }


    void EnergyEvent() {
        if (Slot.activeUnit.noFinger) return;
        infoText.SetValue(System.String.Format("{0} was cutting carrots but a sudden movement sliced {1} middle finger away. ", Slot.activeUnit?.unit_name, Slot.activeUnit.possessive_selection));
        Slot.activeUnit.addToBio(System.String.Format("{0} lost a finger while cutting carrots", Slot.activeUnit?.pronoun));
        Slot.activeUnit.noFinger = true;
        infoEvent.Raise();
    }


    void MotorEvent() {
        if (Slot.activeUnit.noFinger) return;
        infoText.SetValue(System.String.Format("{0} was cutting carrots but a sudden movement sliced {1} middle finger away. ", Slot.activeUnit?.unit_name, Slot.activeUnit.possessive_selection));
        Slot.activeUnit.addToBio(System.String.Format("{0} lost a finger while cutting carrots", Slot.activeUnit?.pronoun));
        Slot.activeUnit.noFinger = true;
        infoEvent.Raise();
    }



    void SteamEvent() {
        if (Slot.activeUnit.noFinger) return;
        infoText.SetValue(System.String.Format("{0} was cutting carrots but a sudden movement sliced {1} middle finger away. ", Slot.activeUnit?.unit_name, Slot.activeUnit.possessive_selection));
        Slot.activeUnit.addToBio(System.String.Format("{0} lost a finger while cutting carrots", Slot.activeUnit?.pronoun));
        Slot.activeUnit.noFinger = true;
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
        fireImage.gameObject.SetActive(isBroken);
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
}
