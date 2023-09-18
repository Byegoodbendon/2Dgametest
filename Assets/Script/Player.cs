using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Collider2D coll;
    private Collider2D pcoll;
    private Collider2D collbox;
    private Animator anim;
    public float speed;
    private float speedcoeficient;
    public float jumpforce;
    private bool jumppressed; 
    private bool crouchpressed;
    public LayerMask ground;   //判斷地面圖層
    public int Cherry;
    public int Diamond;
    private int jumpcounter;
    public Text cherrynum;
    public Text dianum;
    private bool ishurt; //預設為false
    private Time hurtcooldown;
    void Start()
    {
       //rb = GetComponent<Rigidbody2D>();
       rb = GetComponent<Rigidbody2D>();
       coll = GetComponent<CircleCollider2D>();
       collbox = GetComponent<BoxCollider2D>();
       pcoll = GetComponent<PolygonCollider2D>();
       anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //跳躍判斷
       if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
       {
         jumppressed = true;
       }
        //蹲下判斷      
       if(Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("crouching",true);
        }
        else
        {
            anim.SetBool("crouching",false);
        }
      
    }

    void FixedUpdate()
    {
        if(!ishurt)
        {
          movement();
        }
        Jump();
        SwitchAnim();
        speedco();

       
        //groundmovement();
        
        
    }
    //蹲下狀態調整
    void speedco()
    {
        if(coll.IsTouchingLayers(ground) && Input.GetKey(KeyCode.LeftControl))
        {
            speedcoeficient = 0.6f;
            collbox.offset = new Vector2(collbox.offset.x, -0.5f);
        }
        else
        {
            speedcoeficient = 1.0f;
            collbox.offset = new Vector2(collbox.offset.x, -0.182f);
        }
    }
    //角色移動
    void movement()
    {
       float horizontalmove = Input.GetAxis("Horizontal");
       float facedirection = Input.GetAxisRaw("Horizontal");
       
       //角色移動
       if(horizontalmove != 0)
       {
         rb.velocity = new Vector2(horizontalmove * speed * speedcoeficient,rb.velocity.y);
         anim.SetFloat("running", Mathf.Abs(horizontalmove));
       }
       
       //角色方向
       if(facedirection != 0)
       {
        transform.localScale = new Vector3(facedirection,1,1);
       }
        
    }
    
    //角色跳躍
    void Jump()
    {
        if(jumppressed && !ishurt)
       {
           //rb.AddForce(Vector2.up*jumpforce*Time.deltaTime);
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        jumppressed = false;
        anim.SetBool("jumping",true);
           //System.Console.WriteLine("123");
       }
    }
   
    //動畫切換
    void SwitchAnim()
    {
        if(rb.velocity.y < 0.1f && !pcoll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",true);
        }
        if(ishurt)
        {
            anim.SetBool("hurting",true);
            anim.SetFloat("running",0);
            if(Mathf.Abs(rb.velocity.x)<0.01)
            {
                ishurt = false;
                anim.SetBool("hurting",false);
                anim.SetBool("idleing",true);
            }
        }
        else if(anim.GetBool("jumping"))
        {
            anim.SetBool("idleing",false);
            if(rb.velocity.y <= 0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
            anim.SetBool("idleing",true);
        }
        
    }
    
    //物品蒐集
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag =="Collection")
        {
            Destroy(collision.gameObject);
            Cherry = Cherry+1;
            cherrynum.text = Cherry.ToString();
        }
        if(collision.tag =="collection diamond")
        {
            Destroy(collision.gameObject);
            Diamond = Diamond+1;
            dianum.text = Diamond.ToString();
        }
    }

    //和敵人互動
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(anim.GetBool("falling"))
            {
            Destroy(collision.gameObject);
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * 0.5f);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
            rb.velocity = new Vector2(-7.0f, rb.velocity.y);
            ishurt = true;
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
            rb.velocity = new Vector2(7.0f, rb.velocity.y);
            ishurt = true;
            }
        }
    }
    
    
    

}
