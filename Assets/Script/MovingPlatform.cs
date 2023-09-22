using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform PointA, PointB;
    public float movingSpeed;
    Vector2 targetPos1;
    Vector2 targetPos2;
    Vector2 targetPos;
    
        void Start()
    {
        targetPos1 = PointA.position;
        targetPos2 = PointB.position;
        targetPos = targetPos1;
        
        //Destroy(leftPoint.gameObject);
        //Destroy(rightPoint.gameObject);    
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,targetPos1) < 0.1f) targetPos = targetPos2;
        if(Vector2.Distance(transform.position,targetPos2) < 0.1f) targetPos = targetPos1;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, movingSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);
        }
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
