using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EventTypes {
    fish1,
    fish2,
    fish3,
    duel,
    nigerian,
    motor,
    fishing,
    birthday,
    pipe,
}

public class ShipManager : MonoBehaviour
{
    private List<IShip> shipRooms;
    private List<IEvent> events;
    public ControlRoomScript controlRoom;
    private Dictionary<ShipStat, int> shipStats;
    private bool canCall = true;
    public TextMeshProUGUI depth_text;
    public int depth = 0;
    public List<Unit> active_units = new List<Unit>();
    public Dictionary<ShipStat, int> delta;

    public EventTypes[] event_types = (EventTypes[])System.Enum.GetValues(typeof(EventTypes));
    public StringVariable infoText;
    public Choice positiveChoice;
    public Choice negativeChoice;
    public GameEvent choiceEvent;
    public GameEvent infoEvent;
    public StringVariable depth_var;
    bool canEvent = false;

    public bool monster1 = false;
    public bool monster2 = false;
    public bool monster3 = false;

    // Start is called before the first frame update
    void Awake()
    {
        events = new List<IEvent>(GetComponentsInChildren<IEvent>());
        shipStats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 30 },
            { ShipStat.Heat, 30 },
            { ShipStat.Energy, 30 },
            { ShipStat.Motors, 4 },
            { ShipStat.Happiness, 50 },
            { ShipStat.Buoyancy, Random.Range(1,50) },
            { ShipStat.Antigravity, Random.Range(95,362) },
            { ShipStat.Evilness, Random.Range(0,101) }
        }; 
        delta = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 },
            { ShipStat.Buoyancy, 0 },
            { ShipStat.Antigravity, 0 },
            { ShipStat.Evilness, 0 }
        };

        shipRooms = new List<IShip>(GetComponentsInChildren<IShip>());
        StartCoroutine(TestEvent());


    }

    IEnumerator TestEvent() {
        yield return new WaitForSeconds(15);
        canEvent = true;
    }

    public Dictionary<ShipStat, int> GetStats() {
        return shipStats;
    }

    public Dictionary<ShipStat, int> GetDelta() {
        return delta;
    }

    public Unit[] GetTwoUnits() {
        var i = Random.Range(0, 3);
        Unit[] temp = new Unit[] { active_units[i], active_units[(i + 1) % 3] };
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCall) {
            canCall = false;
            StartCoroutine(Tick());
        }
        depth_text.text = depth.ToString() + " m.";
        
    }

    void GenerateEvents() {
        var x = Random.Range(0, 101);
        print("Random ship: " + x);
        if (x < 4) {
            var selected_event = event_types[Random.Range(0, event_types.Length)];
            switch (selected_event) {
                case EventTypes.duel:
                    DuelEvent();
                    break;
                case EventTypes.motor:
                    MotorEvent();
                    break;
                case EventTypes.nigerian:
                    NigerianEvent();
                    break;
                case EventTypes.fishing:
                    FishEvent();
                    break;
                case EventTypes.birthday:
                    BirthdayEvent();
                    break;
                case EventTypes.pipe:
                    PipeEvent();
                    break;
            }
        }
    }

    void DuelEvent() {
        var i = Random.Range(0, 3);
        var unit_1 = active_units[i];
        var unit_2 = active_units[(i + 1) % 3];
        var str1 = Random.Range(1, 21) + unit_1.GetStats()[UnitStat.combat];
        var str2 = Random.Range(1, 21) + unit_2.GetStats()[UnitStat.combat];
        string[] body = { "abdomen", "right leg", "neck", "front", "back", "left leg", "torso", "right arm", "left arm"};
        var random_body = body[Random.Range(0, body.Length)];
        if (str1 > str2) {

            infoText.SetValue(System.String.Format("Due to deteriorating psychological conditions {0} challenged {1} to a duel and won. {1} has been left with a scar on {2} {3}.\n[{2} stats are halved]",
                                                    unit_1.unit_name, unit_2.unit_name, unit_2.possessive_selection, random_body));
            unit_2.addToBio(System.String.Format("{0} got a scar while fighting {1}. ", unit_2.pronoun, unit_1.name));
            unit_1.addToBio(System.String.Format("{0} challenged {1} to a duel and won. ", unit_1.pronoun, unit_2.name));
            unit_2.HalfStats();
        }
        else {
            infoText.SetValue(System.String.Format("Due to deteriorating psychological conditions {0} challenged {1} to a duel and lost. {0} has been left with a scar on {2} {3}.\n[{2} stats are halved]",
            unit_1.unit_name, unit_2.unit_name, unit_1.possessive_selection, random_body));
            unit_1.addToBio(System.String.Format("{0} got a scar while fighting {1}. ", unit_1.pronoun, unit_2.name));
            unit_1.addToBio(System.String.Format("{0} challenged {1} to a duel and lost. ", unit_1.pronoun, unit_2.name));
            unit_2.addToBio(System.String.Format("{0} won a duel against {1}. ", unit_2.pronoun, unit_1.name));
            unit_1.HalfStats();
        }   
        infoEvent.Raise();
    }

    
    void NigerianEvent() {
        Unit u = active_units[Random.Range(0, active_units.Count)];
        infoText.SetValue(System.String.Format("{0} discovered that he has a relative in Nigeria that died, leaving {1} with a big inheritance. Claim it?", u.unit_name, u.himhers));

        positiveChoice.SetChoice((infoText) => {
            infoText.SetValue(System.String.Format("Can't believe you fell for it. You lost 700$, loser"));
        });
        negativeChoice.SetChoice((infoText) => {
            infoText.SetValue(System.String.Format("I guess someone else will have to have it", u.unit_name));
        });
        choiceEvent.Raise();
    }

    void FishEvent() {
        Unit u = active_units[Random.Range(0, active_units.Count)];
        infoText.SetValue(System.String.Format("{0} craves for food and wants to go fishing. Allow {1}?", u.unit_name, u.himhers));

        positiveChoice.SetChoice((infoText) => {
            shipStats[ShipStat.Energy] -= 10;
            shipStats[ShipStat.Food] += 10;
            infoText.SetValue(System.String.Format("{0} opens the door and floods an entire room of the submarine. At least you have some fish now...\n-10 Energy\n+10 Food", u.unit_name));
        });
        negativeChoice.SetChoice((infoText) => {
            shipStats[ShipStat.Happiness] -= 10;
            infoText.SetValue(System.String.Format("{0} is unhappy about the choice\n-10 Happiness", u.unit_name));
        });
        choiceEvent.Raise();
    }

    void BirthdayEvent() {
        Unit u = active_units[Random.Range(0, active_units.Count)];
        if (u.birthday) return;
        infoText.SetValue(System.String.Format("It's {0}'s birthday today! There's a big party in the Lounge", u.unit_name));
        u.addToBio(u.pronoun + " celebrated his birthday on the submarine");
        u.birthday = true;
        infoEvent.Raise();
    }

    void PipeEvent() {
        foreach (Unit u in active_units) {
            if (u.GetStats()[UnitStat.engineering] > 5) {
                    
                infoText.SetValue("One of your pipes has been destoryed and the water filled the submarine but " + u.unit_name +  " managed to promptly solve the issue thanks to his skills");
                u.GetStats()[UnitStat.xp] += 60;
            }
            else {
                infoText.SetValue("One of your pipes has been destoryed and the water filled the submarine. One of the motors is now out of order");
                shipStats[ShipStat.Motors] -= 1;
            }
        }
        infoEvent.Raise();
    }

    void MotorEvent() {
        foreach (Unit u in active_units) {
            if (u.GetStats()[UnitStat.engineering] > 5) {

                shipStats[ShipStat.Motors] += 1;
                infoText.SetValue("You found a broken motor in one of the closets. " + u.unit_name + " managed to repair it thanks to his skills\n[+1 Motor]");
                u.GetStats()[UnitStat.xp] += 60;
                break;
            }
            else {
                infoText.SetValue("You found a broken motor in one of the closets but none of your units is knowledgeable enough to fix it");
                    
            }
        }
        infoEvent.Raise();
    }

    void MonsterEvent2() {
        Unit maxStr = active_units[0];
        foreach (Unit u in active_units){
            if (u.GetStats()[UnitStat.combat] > maxStr.GetStats()[UnitStat.combat]) maxStr = u;
        }
        var rat_strength = Random.Range(1, 14);
        var player_strength = Random.Range(1, 21) + maxStr.GetStats()[UnitStat.combat];
        if (rat_strength > player_strength) {
            infoText.SetValue(System.String.Format("Crawling crabs jumped on your submarine from the rocks around. {0}, with all of {3} might fought the creatures.\nCrabs' strength: {1}\n{0}'s strength: {2}\n{0} gained XP", maxStr.unit_name, rat_strength, player_strength));
            maxStr.GetStats()[UnitStat.xp] += 250;
        }
        else {
            var motor = maxStr.GetStats()[UnitStat.engineering] > 6 ? "" : "\nA motor has also been destroyed";
            infoText.SetValue(System.String.Format("Giant, snake-like creatures, appeared from the depths and attacked the ship. {0} tried scaring them off but his combat prowless wasn't enough.\nRats strength: {1}\n{0} strength: {2}\n[-30 Energy]{3}", maxStr.unit_name, rat_strength, player_strength, motor));
            shipStats[ShipStat.Energy] -= 15;
            if (maxStr.GetStats()[UnitStat.engineering] < 6) shipStats[ShipStat.Motors] -= 1;


        }
            
    }

    void MonsterEvent1() {
        Unit maxStr = active_units[0];
        foreach (Unit u in active_units) {
            if (u.GetStats()[UnitStat.combat] > maxStr.GetStats()[UnitStat.combat]) maxStr = u;
        }
        var rat_strength = Random.Range(1, 12);
        var player_strength = Random.Range(1, 21) + maxStr.GetStats()[UnitStat.combat];
        if (rat_strength > player_strength) {
            infoText.SetValue(System.String.Format("Giant, snake-like creatures, appeared from the depths and attacked the ship. {0} used all of his combat capabilities and won.\nSnakes' strength: {1}\n{0} strength: {2}\n{0} gained XP", maxStr.unit_name, rat_strength, player_strength));
            maxStr.GetStats()[UnitStat.xp] += 150;
        }
        else {
            infoText.SetValue(System.String.Format("Giant, snake-like creatures, appeared from the depths and attacked the ship. {0} tried scaring them off but his combat prowless wasn't enough.\nRats strength: {1}\n{0} strength: {2}\n[-20 Energy]", maxStr.unit_name, rat_strength, player_strength));
            shipStats[ShipStat.Energy] -= 10;

        }

    }

    void MonsterEvent3() {
        Unit maxStr = active_units[0];
        foreach (Unit u in active_units) {
            if (u.GetStats()[UnitStat.combat] > maxStr.GetStats()[UnitStat.combat]) maxStr = u;
        }
        var rat_strength = Random.Range(1, 16) + 5;
        var player_strength = Random.Range(1, 21) + maxStr.GetStats()[UnitStat.combat];
        var crush = (rat_strength > 15) ? "\nThe whale crushed {0}. Stats halved." : "";
        if (rat_strength > player_strength) {
            infoText.SetValue(System.String.Format("You encountered an Elder Whale. A wanering monstrousity. Luckily {0} strenght was enough and after a fierce battle you survived.\nWhale strength: {1}\n{0} strength: {2}\n{0} gained XP.", maxStr.unit_name, rat_strength, player_strength));
            maxStr.GetStats()[UnitStat.xp] += 150;
        }
        else {
            infoText.SetValue(System.String.Format("You encountered an Elder Whale. A wanering monstrousity. It's size and it's knowledge were enough to defeat {0}.\nwhale strength: {1}\n{0} strength: {2}\n[-40 Energy]{3}", maxStr.unit_name, rat_strength, player_strength, crush));
            if (rat_strength > 15) maxStr.HalfStats();
            shipStats[ShipStat.Energy] -= 20;

        }

    }



    public int controlUnit = 1;

    IEnumerator Tick() {
        if(canEvent)GenerateEvents();
        var temp = new Dictionary<ShipStat, int> {
            { ShipStat.Food, 0 },
            { ShipStat.Heat, 0 },
            { ShipStat.Energy, 0 },
            { ShipStat.Motors, 0 },
            { ShipStat.Happiness, 0 },
            { ShipStat.Buoyancy, 0 },
            { ShipStat.Antigravity, 0 },
            { ShipStat.Evilness, 0 }
        };

        if(depth > 300 && !monster1) {
            monster1 = true;
            MonsterEvent1();
        }

        if(depth > 600 && !monster2) {
            monster2 = true;
            MonsterEvent2();
        }

        if(depth > 900 && !monster3) {
            monster3 = true;
            MonsterEvent3();    
        }
        controlUnit = controlRoom.hasUnit ? 1 : 0;
        var shitty_vars = new Dictionary<ShipStat, int> {
             { ShipStat.Buoyancy, Random.Range(-1,2) },
            { ShipStat.Antigravity, Random.Range(-1,2) },
            { ShipStat.Evilness, Random.Range(-1,2) }
        };

        SetStats(shitty_vars, temp);



        if(controlRoom.hasUnit && !controlRoom.isBroken)depth += 4 * shipStats[ShipStat.Motors] - (int)(shipStats[ShipStat.Buoyancy] * 0.1f);

        depth_var.SetValue(depth.ToString());
        foreach (IShip s in shipRooms) {
            SetStats(s.Tick(), temp);
            s.ResetStats();
        }
       
        if (events != null) { 
            foreach (IEvent e in events) {
                SetStats(e.Tick(), temp);
            }
        }
        events = new List<IEvent>(GetComponentsInChildren<IEvent>());
        Dictionary<ShipStat, int> tickStats = new Dictionary<ShipStat, int> {
            { ShipStat.Food, -active_units.Count},
            { ShipStat.Heat, -3 },
            { ShipStat.Energy, -Random.Range(1,5) },
            { ShipStat.Happiness, -Random.Range(1,4) },

        };
        SetStats(tickStats, temp);
        delta = temp;
        yield return new WaitForSeconds(5f);
        canCall = true;
        yield return null;
    }  
    
    void SetStats(IDictionary<ShipStat, int> dict, IDictionary<ShipStat, int> temp) {
        foreach(ShipStat s in dict.Keys) {
            //print(temp[s]);
            temp[s] += dict[s];
            shipStats[s] += dict[s];
        }
    }
}