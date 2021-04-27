using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipDeath : MonoBehaviour
{
    public ShipManager ship;
    Dictionary<ShipStat, int> stats;
    bool generalWarning;
    bool happinessWarning;
    bool motorWarning;
    bool sound;
    public GameEvent deathEvent;
    public AudioClip alarm;
    public GameObject generalWarningIcon;
    public GameObject happWarn, foodWarn, enerWarn, heatWarn;
    AudioSource src;
    public bool maxAudio = false;
    public GameObject death;
    public bool won = false;
    // Start is called before the first frame update
    void Start()
    {
        generalWarning = false;
        ship = GetComponent<ShipManager>();
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        stats = ship.GetStats();
        CheckWarning(stats);
        CheckDeath(stats);
        generalWarningIcon.gameObject.SetActive(generalWarning);
        if (generalWarning) {
            
            if(!maxAudio)
                StartCoroutine(fadeSource(src,0,.5f,1));
        }
        else {
            if(maxAudio)
                StartCoroutine(fadeSource(src, .5f, 0, 1));
        }

        if (ship.depth > 1000 && !won) StartCoroutine(Won());
    }


    IEnumerator fadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration) {
        float startTime = Time.time;

        while (true) {

            if (duration == 0) {
                
                sourceToFade.volume = endVolume;
                maxAudio = startVolume > endVolume ? false : true;
                break;//break, to prevent division by  zero
            }
            float elapsed = Time.time - startTime;

            sourceToFade.volume = Mathf.Clamp01(Mathf.Lerp(startVolume,
                                                            endVolume,
                                                            elapsed / duration));

            if (sourceToFade.volume == endVolume) {
                maxAudio = startVolume > endVolume ? false : true;
                break;
            }
            yield return null;
        }//end while
    }



    void CheckWarning(Dictionary<ShipStat, int> s) {
        generalWarning = false;
        motorWarning = false;
        happinessWarning = false;
        if (s[ShipStat.Motors] < 2) motorWarning = true;
        if (s[ShipStat.Energy] < 10) generalWarning = true;
        if (s[ShipStat.Happiness] < 20) happinessWarning = true;
        if (s[ShipStat.Food] < 10) generalWarning = true;
        if (s[ShipStat.Heat] < 10) generalWarning = true;

        foodWarn.SetActive(s[ShipStat.Food] < 10);
        heatWarn.SetActive(s[ShipStat.Heat] < 10);
        happWarn.SetActive(s[ShipStat.Happiness] < 10);
        enerWarn.SetActive(s[ShipStat.Energy] < 10);
    }

    void CheckDeath(Dictionary<ShipStat, int> s) {
        if (s[ShipStat.Energy] < 0 || s[ShipStat.Happiness] < 0 || s[ShipStat.Heat] < 0 || s[ShipStat.Food] < 0) {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death() {
        yield return new WaitForSeconds(10);
        death.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator Won() {
        won = true;
        death.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Victory");
    }


}
