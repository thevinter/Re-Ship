using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public enum FadeAction {
    FadeIn,
    FadeOut,
    FadeInAndOut,
    FadeOutAndIn
}


public class ImageFade : MonoBehaviour {
    [Tooltip("The Fade Type.")]
    [SerializeField] private FadeAction fadeType;

    [Tooltip("the image you want to fade, assign in inspector")]
    [SerializeField] private Image img;


    public void Start() {
        switch (fadeType) {
            case FadeAction.FadeOut:
                StartCoroutine(FadeOut());
                break;
            case FadeAction.FadeIn:
                StartCoroutine(FadeIn());
                break;
        }
    }

    public void StartFadeIn() {
        StartCoroutine(FadeIn());
    }

    // fade from transparent to opaque
    IEnumerator FadeIn() {

        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(img.color.r, img.color.g, img.color.b, i);
            yield return null;
        }

    }

    // fade from opaque to transparent
    IEnumerator FadeOut() {
        // loop over 1 second backwards
        for (float i =1 ; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(img.color.r, img.color.g, img.color.b, i);
            yield return null;

        }
        img.gameObject.SetActive(false);
    }

    IEnumerator FadeInAndOut() {
        // loop over 1 second
        for (float i = 0; i <= 2; i += Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        //Temp to Fade Out
        yield return new WaitForSeconds(1);

        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator FadeOutAndIn() {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        //Temp to Fade In
        yield return new WaitForSeconds(1);

        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

}