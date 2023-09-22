using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Enemy
{
    public Transform leftPoint, rightPoint;
    float leftx, rightx;
    bool faceleft = true;
    public float runSpeed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }
    void Update()
    {
        if(!stopupdate)
        {
            faceDirection();
            movement();
        }
        
    }
    void faceDirection()
    {
        if(transform.position.x < leftx)
        {
            faceleft = false;
            transform.localScale = new Vector3(-1,1,1);
        }
        else if(transform.position.x > rightx)
        {
            faceleft = true;
            transform.localScale = new Vector3(1,1,1);
        }
        

    }
    void movement()
    {
        if(faceleft)
        {
            rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
        }
        else if(!faceleft)
        {
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
        }
        
    }
}
