using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KitchenEvents {
    fire,
    banquet,
    finger,
    rats,
}

public class ShipKitchen : MonoBehaviour, IShip {

    public ItemSlot Slot { get => slot; }
    public ItemSlot slot;
    public Image FireImage { get => fireImage; set => throw new System.NotImplementedException(); }
    public Image fireImage;


    public KitchenEvents[] events = (KitchenEvents[])System.Enum.GetValues(typeof(KitchenEvents));


    public GameEvent infoEvent;
    public GameEvent choiceEvent;
    public Choice positiveChoice;
    public Choice negativeChoice;
    public StringVariable infoText;



    private bool canEvent = false;

    public bool isBroken = true;
    public int repaired = 0;
    public Dictionary<ShipStat, int> stats;
    void Start() {
        slot = GetComponent<ItemSlot>();
        StartCoroutine(Allow());
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 }
        };
        //isBroken = true;
    }

    IEnumerator Allow() {
        yield return new WaitForSeconds(10);
        canEvent = true;
    }

    public IDictionary<ShipStat, int> Tick() {
    
        if (!isBroken) {
            if(Slot.activeUnit != null) { 
                int food = Slot.activeUnit.unit_type == UnitType.Cook ? 5 : (2 + Slot.activeUnit.GetStats()[UnitStat.cooking]);
                stats[ShipStat.Food] += food;
            }
            else {
                stats[ShipStat.Food] += 0;
            }
            if (canEvent) CheckForRandomEvent();
        }
        else {
            if(Slot.activeUnit != null) {
                var x = Slot.activeUnit.unit_type == UnitType.Engingeer ? 5 : 3;
                repaired += x + Slot.activeUnit.GetStats()[UnitStat.engineering];
            }
            if(repaired > 20) {
                isBroken = false;
                repaired = 0;
            }
            ResetStats();
        }

        //print("Sent After: " + stats[ShipStat.Food]);
        return stats;
    }

    void CheckForRandomEvent() {
        int result = Random.Range(0, 101);
        int treshold = Slot.activeUnit?.unit_type == UnitType.Cook ? 2 : 2;
        print("Random kitchen" + result);
        if (result < treshold) {
            var selected_event = events[Random.Range(0, events.Length)];
            switch (selected_event) {
                case KitchenEvents.banquet:
                    if (Slot.activeUnit != null) BanquetEvent();
                    break;
                case KitchenEvents.finger:
                    if(Slot.activeUnit != null) FingerEvent();
                    break;
                case KitchenEvents.fire:
                    FireEvent();
                    break;
                case KitchenEvents.rats:
                    RatEvent();
                    break;
            }
        }

    }

    void RatEvent() {
        if(Slot.activeUnit == null) {
            infoText.SetValue("Rats appeared in your kitchen. Since you had no one there they stole most of your food");
            stats[ShipStat.Food] += -10;
        }
        else {
            var rat_strength = Random.Range(1,12);
            var player_strength = Random.Range(1, 21) + Slot.activeUnit.GetStats()[UnitStat.combat];
            if(rat_strength < player_strength) {
                infoText.SetValue(System.String.Format("Rats appeared in your kitchen. {0} tried scaring them off and won.\nRats strength: {1}\n{0} strength: {2}", Slot.activeUnit.unit_name, rat_strength, player_strength));
            }
            else {
                infoText.SetValue(System.String.Format("Rats appeared in your kitchen. {0} tried scaring them off and lost taking damage.\nRats strength: {1}\n{0} strength: {2}", Slot.activeUnit.unit_name, rat_strength, player_strength));
                stats[ShipStat.Food] += -10;
                Slot.activeUnit.GetStats()[UnitStat.hp] += -Random.Range(1, 4);
            }
        }
    }

    void FireEvent() {
        infoText.SetValue("Your kitchen caught on fire. Too bad you weren't insured. Do you want to try and rescue some food?");

        positiveChoice.SetChoice(
            (infoText) => {
                var x = Random.Range(15, 25); 
                infoText.SetValue(System.String.Format("You tried rescuing some food. You managed to scavenge {0} pieces of food but the kitchen is now broken.", x));
                stats[ShipStat.Food] += x;
                isBroken = true;
            });
        negativeChoice.SetChoice((infoText) => {
            infoText.SetValue("You left the fire calm down a bit, then you used extinguishers to put it out. The food in the kitchen got completely destroyed but at least the kitchen is fine.");
        });
        choiceEvent.Raise();
    }


    void FingerEvent() {
        if (Slot.activeUnit.noFinger) return;
        infoText.SetValue(System.String.Format("{0} was cutting carrots but a sudden movement sliced {1} middle finger away. ", Slot.activeUnit?.unit_name, Slot.activeUnit.possessive_selection));
        Slot.activeUnit.addToBio(System.String.Format("{0} lost a finger while cutting carrots", Slot.activeUnit?.pronoun));
        Slot.activeUnit.noFinger = true;
        infoEvent.Raise();
    }

    
    

    void BanquetEvent() {
        var pizza = Slot.activeUnit.GetStats()[UnitStat.italy] > 5 ? " pizza" : "";

        infoText.SetValue(System.String.Format("{0} organized a{1} banquet for everyone. Invite everyone else?", Slot.activeUnit.unit_name, pizza));
        
        positiveChoice.SetChoice(
            (infoText) => {
                infoText.SetValue("Everyone was happy for the banquet. They ate a lot");
                stats[ShipStat.Happiness] += Random.Range(5, 10);
                print("Before: " + stats[ShipStat.Food]);
                stats[ShipStat.Food] -= Random.Range(5, 10);
                print("After: " + stats[ShipStat.Food]);
            });
        negativeChoice.SetChoice((infoText) => {
            infoText.SetValue("All the food had to be repurposed. The crew became unhappy after learning everything has been canceled");
            stats[ShipStat.Happiness] += -Random.Range(5, 10);
            stats[ShipStat.Food] += Random.Range(5, 10);
        });
        choiceEvent.Raise();
    }

    // Update is called once per frame
    void Update()
    {

        FireImage.gameObject.SetActive(isBroken);

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
