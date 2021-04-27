using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowImagesScript : MonoBehaviour
{
    public GameObject[] sprites;
    public bool canShow = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartImage());
    }

    // Update is called once per frame
    void Update()
    {
        if (canShow) {
            canShow = false;
            StartCoroutine(ShowSprite(sprites[Random.Range(0, sprites.Length)]));
        }
    }

    IEnumerator StartImage() {
        canShow = false;
        yield return new WaitForSeconds(Random.Range(30, 60));
        canShow = true;
        yield return null;
    }

    IEnumerator ShowSprite(GameObject s) {
        s.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        s.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        s.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.5f, 2));
        s.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        s.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.5f, 2));
        s.SetActive(false);
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        s.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.5f, 2));
        s.SetActive(false);
        yield return new WaitForSeconds(Random.Range(20, 40));
        canShow = true;
        yield return null;
    }
}
