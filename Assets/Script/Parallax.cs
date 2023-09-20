using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
   public Transform cameraPosition;
   public float moveRate = 0.0f;
   float startPointX;
   float startPointY;
   public bool lockY;//false
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
    }

    void FixedUpdate()
    {
        if(lockY)
        {
        transform.position = new Vector2(startPointX + cameraPosition.position.x * moveRate, cameraPosition.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX + cameraPosition.position.x * moveRate, startPointY + cameraPosition.position.y * moveRate);

        }
    }
}
