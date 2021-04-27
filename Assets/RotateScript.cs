using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateScript : MonoBehaviour
{
    public float speed;
    private float x;
    private Unit u;
    private Image i;
    // Start is called before the first frame update
    void Start()
    {
        u = GetComponentInParent<Unit>();
        i = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (u.GetStats()[UnitStat.skillpoints] > 0) i.color = new Color(1, 0, 0);
        else i.color = new Color(1, 1, 1);
        x += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, 0, x);
        //transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
