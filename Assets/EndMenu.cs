using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public StringVariable totalDepth;
    public TextMeshProUGUI text;
    public AudioClip[] submarines;
    public AudioClip menu;
    public CrossFadeAudio fade;

    public void Restart() {
        fade.CrossFade(submarines[Random.Range(0,submarines.Length)], 1, 3);
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        fade = GameObject.FindGameObjectWithTag("Music").GetComponent<CrossFadeAudio>();
        fade.CrossFade(menu, 1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Total depth reached: " + totalDepth.GetValue();
    }
}
