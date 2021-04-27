using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitType {
    Cook,
    Engingeer,
    Electrician,
    Explorer
}

public enum UnitStat {
    cooking,
    engineering,
    combat,
    skillpoints,
    xp,
    level,
    hp,
    buoyancy,
    love,
    pancakes,
    italy,
    happiness
}



public class Unit : MonoBehaviour
{
    public GameObject levelUp;
    public string unit_name;
    public string unit_surname;
    public int unit_sex;
    public string bio;
    public bool noFinger = false;
    public bool broken = false;
    private Dictionary<UnitStat, int> stats;
    public UnitType unit_type;
    public GameObject rotating_selection;
    private Image sr;
    public bool birthday = false;
    private string[] intros = {
        "{0} {1} was born on the {2} of {3} {4}. ",
        "The identity of {0} is unknown. No one knows where or when {1} was born. ",
        "{0} {1} is an intrepid explorer born in the year {4}. ",
    };

    private string[] months = { "Genuary", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    private string about = "{0} is {1}. {0} has {2} hair and {0} is {3}. {4} favorite thing is {5} and {0} likes {6}. {4} greatest fear is {7}. ";

    private string[] qualities = {"quiet", "timid", "shy", "vibrant", "childish", "cynical", "lazy", "motivated", "trustwhorty", "moody", "nervous",
                                  "weak", "strong", "based", "active", "considerate", "diplomatic", "mature", "fat", "beautiful", "ugly", "stupid",
                                  "smart", "hairy", "psychopat"};

    private string[] colors = { "black", "blond", "white", "red", "green", "auburn", "ginger red", "light blond", "gray", "chestnut", "brown" };
    private string[] physical = { "tall", "short", "small", "big", "averagely tall" };
    private string[] things = {"singing", "videogames", "spiders", "heights", "depths", "colors", "cars", "programming", "horses", "animals", "driving", "swimming",
                                "sharks", "whales", "fish", "fishing", "dancing", "music discs", "jumping", "fighting", "television", "drawing", "stitching",
                                "surgeries", "doctors", "clowns", "snakes", "musical instruments", "electricity", "firefighters", "holes", "monsters", "books"};

    public void addToBio(string v) {
        bio += v;
    }

    private string about_2 = "{0} started working as a {1} {2} months ago. ";
    private string about_3 = "{0} greatest wish is to {1}. ";
    private string[] wishes = { "become rich", "overcome his fears", "kill someone", "become famous", "watch a submarine sink", "sing for the queen",
                                "write a book", "be chained", "fall in love", "become a policeman", "discover a treasure", "become smart", 
                                "feel happy", "compose an album", "become president", "rob a bank"};

    private string[] sex = { "He", "She" };
    public string pronoun;
    private string[] possessive_sex = { "His", "Her" };
    public string himhers;
    private string[] sexo = { "Him", "Her" };
    public string possessive_selection;


    private string[] male_names = { "Mark", "Tony", "Vinter", "Lukasz", "Viper", "Cipher", "Reaper", "Locus", "Scab", "Troy", "Gian", "Edgar", "Mark" };
    private string[] female_names = { "Luna", "Curse", "Fiona", "Tide", "Lucia", "Marip", "Sparrow", "Ludie", "Georgianna", "Sally", "Gemma", "Linda" };
    private string[] surnames = { "Zen", "Snape", "Ross", "Quint", "Brown", "Haley", "Heist", "Burley", "Blackjack", "McBalls", "Kinsley", "Swindler", "Burton" };

    public void HalfStats() {
        List<UnitStat> us = new List<UnitStat>(stats.Keys);
        foreach(UnitStat s in us) {
            var temp = stats[s];
            stats[s] = temp / 2;
        }
    }

    private void GenerateUnit() {
        sr = GetComponent<Image>();
        
        unit_sex = Random.Range(0, 2);
        if(unit_sex == 0) { //Male
            unit_name = male_names[Random.Range(0, male_names.Length)];
        }
        else {//female
            unit_name = female_names[Random.Range(0, female_names.Length)];

        }
        pronoun = sex[unit_sex];
        possessive_selection = possessive_sex[unit_sex];
        himhers = sexo[unit_sex];
        unit_surname = surnames[Random.Range(0, surnames.Length)];

        stats = new Dictionary<UnitStat, int> {
            { UnitStat.cooking, Random.Range(0, 3) },
            { UnitStat.engineering, Random.Range(1, 3) },
            { UnitStat.combat, Random.Range(0, 3) },
            { UnitStat.skillpoints, 0 },
            { UnitStat.xp, 0 },
            { UnitStat.level, 1 },
            { UnitStat.hp, 10 },
            { UnitStat.italy, Random.Range(0,3) },
            { UnitStat.happiness, 5 }
        };
        System.Type type = typeof(UnitType);
        System.Array values = type.GetEnumValues();
        int index = Random.Range(0,values.Length);
        unit_type = (UnitType)values.GetValue(index);
        switch (unit_type) {
            case UnitType.Cook:
                sr.color = new Color(0, 0.8f, 0);
                break;
            case UnitType.Electrician:
                sr.color = new Color(0, 0.7f, 0.7f);
                break;
            case UnitType.Explorer:
                sr.color = new Color(0.6f, 0, 0.6f);
                break;
            case UnitType.Engingeer:
                sr.color = new Color(0, 0, 0.6f);
                break;
        }
        GenerateBio();
    }

    public Dictionary<UnitStat, int> GetStats() {
        return stats;
    }


    private void Update() {
        rotating_selection.SetActive(gameObject.CompareTag("Selected"));
        if (stats[UnitStat.xp] >= 100 * stats[UnitStat.level]) LevelUp();
        levelUp.SetActive(stats[UnitStat.skillpoints] > 0);
    }

    public void LevelUp() {
        stats[UnitStat.skillpoints] += 1;
        stats[UnitStat.level] += 1;
        stats[UnitStat.xp] = 0;
    }

    public void SetStat(UnitStat s, int v) {
        stats[s] += v;
        if(stats[UnitStat.skillpoints] > 0) stats[UnitStat.skillpoints] -= 1;
    }

    // Update is called once per frame
    void Awake()
    {
        GenerateUnit();
    }

    private void GenerateBio() {
        bio = "";                       //"                                     {0}             {1} was born on the {2} of              {3}                              {4} in a world full of light. ",
        bio = bio + System.String.Format(intros[Random.Range(0, intros.Length)], unit_name, unit_surname, Random.Range(0, 29), months[Random.Range(0, months.Length)], Random.Range(1940, 1998))
            //                          {0}           is {1}.  {0}                                has {2} hair a
            + System.String.Format(about, pronoun, qualities[Random.Range(0, qualities.Length)], colors[Random.Range(0, colors.Length)],
                                //nd {0} is {3}.                                       {4} favorite thing is {5} and {0} likes {6}. {4} greatest fear is {7}
                                physical[Random.Range(0, physical.Length)], possessive_selection ,things[Random.Range(0, things.Length)], things[Random.Range(0, things.Length)],
                                things[Random.Range(0, things.Length)])
            + System.String.Format(about_2, pronoun, unit_type, Random.Range(1, 13))
            + System.String.Format(about_3, possessive_selection, wishes[Random.Range(0, wishes.Length)]);

        //print(bio); 
    }
}

