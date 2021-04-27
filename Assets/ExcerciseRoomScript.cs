using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ExcerciseEvent {
    broken,
    good_excercise,
    martial,
    fightclub,
    xp,
    pushups,
    breaking
}
public class ExcerciseRoomScript : MonoBehaviour, IShip
{
    public ItemSlot Slot => GetComponent<ItemSlot>();
    private ExcerciseEvent[] events = (ExcerciseEvent[])System.Enum.GetValues(typeof(ExcerciseEvent));

    public GameEvent infoEvent;
    public GameEvent choiceEvent;
    public Choice positiveChoice;
    public Choice negativeChoice;
    public StringVariable infoText;
    public ShipManager ship;
    public Image FireImage { get => fireImage; set => throw new System.NotImplementedException(); }
    public Image fireImage;
    public bool isBroken = false;
    IDictionary<ShipStat, int> stats;
    int repaired = 0;
    bool canEvent = false;
    IEnumerator Allow() {
        yield return new WaitForSeconds(10);
        canEvent = true;
    }
    void Start() {
        StartCoroutine(Allow());
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 }
        };
    }
    public IDictionary<ShipStat, int> Tick() {
        if (!isBroken) {
            if(canEvent) CheckForRandomEvent();
            if (Slot.activeUnit != null) {

                Slot.activeUnit.GetStats()[UnitStat.xp] += 15;
            }
            
        }
        
        else {
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
        return stats;

    }


    void CheckForRandomEvent() {
        int result = Random.Range(0, 101);
        int treshold = Slot.activeUnit?.unit_type == UnitType.Cook ? 3 : 3;
        print("Random excercise" + result);
        if (result < treshold) {
            var selected_event = events[Random.Range(0, events.Length)];
            switch (selected_event) {
                case ExcerciseEvent.good_excercise:
                    if (Slot.activeUnit != null) GoodExcerciseEvent();
                    break;
                case ExcerciseEvent.martial:
                    if (Slot.activeUnit != null) MartialEvent();
                    break;
                case ExcerciseEvent.fightclub:
                    FightClubEvent();
                    break;
                case ExcerciseEvent.xp:
                    if (Slot.activeUnit != null) XPEvent();
                    break;
                case ExcerciseEvent.broken:
                    if (Slot.activeUnit != null) BrokenEvent();
                    break;
                case ExcerciseEvent.breaking:
                    BreakingEvent();
                    break;
            }
        }
    }

        void BreakingEvent() {
            infoText.SetValue(System.String.Format("Some weights caught on fire. Can they even do that? The workout room is broken"));
            isBroken = true;
            infoEvent.Raise();
        }

        void GoodExcerciseEvent() {
            var x = Random.Range(1, 300);
            infoText.SetValue(System.String.Format("{0} had a good training session. {2} gained {1} xp", Slot.activeUnit.unit_name, x, Slot.activeUnit.pronoun));
            Slot.activeUnit.GetStats()[UnitStat.xp] += x;
            infoEvent.Raise();
        }

        void BrokenEvent() {
            if (Slot.activeUnit.broken) return;
            string[] body = { "abdomen", "right leg", "neck", "front", "back", "left leg", "torso", "right arm", "left arm" };
            var x = body[Random.Range(0, body.Length)];

            infoText.SetValue(System.String.Format("{0} broke {1} {2} while he was doing pushups.\n[-1 Combat]", Slot.activeUnit.unit_name, Slot.activeUnit.possessive_selection,x ));
            Slot.activeUnit.addToBio(System.String.Format("{0} broke {1} {2} while excercising", Slot.activeUnit.unit_name, Slot.activeUnit.possessive_selection, x));
            Slot.activeUnit.broken = true;
            Slot.activeUnit.GetStats()[UnitStat.combat] -= 1;
            infoEvent.Raise();
        }

        void XPEvent() {
            infoText.SetValue("{0} is on a streak. {1} might push {2} excercises even further. Risk it?");

            positiveChoice.SetChoice(
                (infoText) => {
                    var x = Random.Range(1, 11);
                    if (x > 5) {
                        var y = Random.Range(20, 200);
                        infoText.SetValue(System.String.Format("It paid off.\n[+{0} xp]", y));
                        Slot.activeUnit.GetStats()[UnitStat.xp] += y;

                    }
                    else {
                        infoText.SetValue(System.String.Format("Ouch, a strained muscle.\n[XP reset]"));
                        Slot.activeUnit.GetStats()[UnitStat.xp] = 0;
                    }
                });
            negativeChoice.SetChoice((infoText) => {
                infoText.SetValue("It's better to take it easy");
            });
            choiceEvent.Raise();
            
        }


        void FightClubEvent() {
            Unit[] x = ship.GetTwoUnits();
            infoText.SetValue(System.String.Format("{0} and {1} organized a fight club in the excercise room. Allow them?", x[0].unit_name, x[1].unit_name));

            positiveChoice.SetChoice(
                (infoText) => {
                    var str1 = Random.Range(1, 21) + x[0].GetStats()[UnitStat.combat];
                    var str2 = Random.Range(1, 21) + x[1].GetStats()[UnitStat.combat];
                    if (str1 > str2) {

                        infoText.SetValue(System.String.Format("{0} won against {1} as the others were watching. {1} is now sad but more knowledgeable.\n{0} gained combat skills.\n",
                                                                x[0].unit_name, x[1].unit_name));
                        x[1].addToBio(System.String.Format("{0} lost in a fight club against {1}. ", x[1].pronoun, x[0].name));
                        x[0].addToBio(System.String.Format("{0} challenged {1} to a fight club and won. ", x[0].pronoun, x[1].name));
                        x[1].GetStats()[UnitStat.happiness] -= 2;
                        x[1].GetStats()[UnitStat.xp] += 50;
                        x[0].GetStats()[UnitStat.combat] += 1;
                    }
                    else {
                        var unit_1 = x[0];
                        var unit_2 = x[1];
                        infoText.SetValue(System.String.Format("{0} won against {1} as the others were watching. {1} is now sad but more knowledgeable.\n{0} gained combat skills.\n",
                                                                 x[0].unit_name, x[1].unit_name));
                        unit_1.addToBio(System.String.Format("{0} got a scar during a fight club with {1}. ", unit_1.pronoun, unit_2.name));
                        unit_1.addToBio(System.String.Format("{0} challenged {1} to a boxing match and lost. ", unit_1.pronoun, unit_2.name));
                        unit_2.addToBio(System.String.Format("{0} won a fight club against {1}. ", unit_2.pronoun, unit_1.name));
                        x[0].GetStats()[UnitStat.happiness] -= 2;
                        x[0].GetStats()[UnitStat.xp] += 50;
                        x[1].GetStats()[UnitStat.combat] += 1;
                    }
                });
            negativeChoice.SetChoice((infoText) => {
                infoText.SetValue("Both of them went to their cabins unhappy but no one got hurt");
                x[0].GetStats()[UnitStat.happiness] -= 1;
                x[1].GetStats()[UnitStat.happiness] -= 1;
            });
            choiceEvent.Raise();
        }


        void MartialEvent() {
            infoText.SetValue(System.String.Format("{0} invented a new martial art. {1} combat skills improved", Slot.activeUnit.unit_name, Slot.activeUnit.possessive_selection));
            Slot.activeUnit.addToBio(System.String.Format("{0} is the inventor of a new martial style", Slot.activeUnit?.pronoun));
            Slot.activeUnit.GetStats()[UnitStat.combat] += 1;

            infoEvent.Raise();
        }

   

    public void Update() {
        fireImage.gameObject.SetActive(isBroken);
    }

    public void ResetStats() {
        stats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 },
            
        };
    }
}
