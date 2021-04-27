using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float speed = 4f;
    Vector3 StartPosition;
    public ShipManager ship;
    public Canvas c;
    // Start is called before the first frame update
    void Start()
    {
        
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //print(ship.controlUnit);
        transform.Translate((Vector3.up * speed * Time.deltaTime * ship.controlUnit * ship.GetStats()[ShipStat.Motors]/2) );
        if (transform.position.y > 10) transform.position = StartPosition;
    }
}
