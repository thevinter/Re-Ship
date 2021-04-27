using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    
    public static AudioClip click;
    public static AudioSource src;
    // Start is called before the first frame update
    void Start()
    {
        click = Resources.Load<AudioClip>("click");
        src = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string v) {
        switch (v) {
            case ("click"):
                src.PlayOneShot(click);
                break;
        }
    }
}
