using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Eagle : MonoBehaviour
{
    private bool faceup = true;
    private float upy,downy;
    public Transform up;
    public Transform down;
    private float movingspeed = 2.0f; 
    
    // Start is called before the first frame update
    void Start()
    {
        upy = up.transform.position.y;
        downy = down.transform.position.y;
        Destroy(up.gameObject);
        Destroy(down.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }
    void movement()
    {
        if(faceup)
        {
            transform.Translate(0,movingspeed * Time.deltaTime,0); 
        }
        else
        {
            transform.Translate(0,-movingspeed * Time.deltaTime,0);
        }
        if(transform.position.y > upy)
        {
            faceup = false;
        }
        else if(transform.position.y < downy)
        {
            faceup = true;
        }

    }
}
