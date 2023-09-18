using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public Transform left;
    public Transform right;
    private float leftx,rightx;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    //移動速度
    private float movingspeed = 3.0f;
    private float jumpingspeed = 4.0f;

    private bool faceleft = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        leftx = left.transform.position.x;
        rightx = right.transform.position.x;
        Destroy(left.gameObject);
        Destroy(right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switchanim();
    }
    
    void movement()
    {
       if(coll.IsTouchingLayers(ground))
       { 
            if(faceleft)
            {
               rb.velocity = new Vector2(-movingspeed,jumpingspeed);
               anim.SetBool("jumping",true);
            
            }
            else if(!faceleft)
            {
               rb.velocity = new Vector2(movingspeed,jumpingspeed);
               anim.SetBool("jumping",true);
            }
        }
    }
    //動畫切換
    void switchanim()
    {
        if(!coll.IsTouchingLayers(ground) && rb.velocity.y < 0.1f)
        {
            anim.SetBool("jumping",false);
            anim.SetBool("falling",true);
            
        }
        if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
            if(rb.position.x < leftx)
            {
               transform.localScale = new Vector3(-1,1,1);
               faceleft = false;
            }
            else if(rb.position.x > rightx)
            {
                transform.localScale = new Vector3(1,1,1);
                faceleft = true;
            }
        }
    }
}
