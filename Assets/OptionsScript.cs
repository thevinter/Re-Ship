using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    public AudioClip[] submarines;
    public CrossFadeAudio fade;

    public void Restart() {
        fade.CrossFade(submarines[Random.Range(0, submarines.Length)], 1, 3);
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        Application.Quit();
    }

    public void Start() {
        fade = GameObject.FindGameObjectWithTag("Music").GetComponent<CrossFadeAudio>();

    }
}
