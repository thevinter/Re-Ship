using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMenuManager : MonoBehaviour
{
    public Animator anim;
    public GameObject startButton;
    public GameObject otherButton1;
    public GameObject otherButton2;
    public GameObject about;
    public GameObject logo;
    public GameObject fade;
    public GameObject audio;
    public CrossFadeAudio cross;

    //public AudioClip menu;
    public AudioClip[] submarines;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene() {
        fade.SetActive(true);
        fade.GetComponent<ImageFade>().StartFadeIn();
        cross.CrossFade(submarines[Random.Range(0, submarines.Length)], 1, 3);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
        yield return null;
    }

    public void ShowAbout(bool what) {
        about.SetActive(what);
    }

    IEnumerator Menu() {
        startButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("animWhale", true);
        
        yield return new WaitForSeconds(1.6f);
        logo.SetActive(true);
        audio.SetActive(true);
        otherButton1.SetActive(true);
        otherButton2.SetActive(true);
        yield return null;
    }

    public void StartMenu() {
        StartCoroutine(Menu());
    }
}
