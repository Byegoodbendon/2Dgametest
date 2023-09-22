using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransformState : MonoBehaviour
{
    public Transform OriginalParent
    {
        get;
        set;
    }
    void Awake() {
        this.OriginalParent = this.transform.parent;
    }
}
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Collider2D pcoll;
    private Collider2D collbox;
    private Animator anim;
    public float speed;
    private float speedcoeficient = 1.0f;
    public float jumpforce;
    private bool jumppressed; 
    private bool isCrouch;
    public Transform cellingCheck;
    public Transform groundCheck;
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
    
       if(Input.GetButtonDown("Jump") && Physics2D.OverlapCircle(groundCheck.position, 1.0f, ground))
       {
         jumppressed = true;
       }
        //蹲下判斷      
       if(Input.GetKey(KeyCode.LeftControl) && Physics2D.OverlapCircle(groundCheck.position, 1.0f, ground))
        {
            isCrouch = true;
            anim.SetBool("crouching",true);
        }
        else if(Physics2D.OverlapCircle(cellingCheck.position , 0.2f, ground) && Physics2D.OverlapCircle(groundCheck.position, 0.5f, ground))
        {
            isCrouch = true;
            anim.SetBool("crouching",true);
        }
        else
        {
            isCrouch = false;
            anim.SetBool("crouching",false);
        }
        //Jump();
      
    }

    void FixedUpdate()
    {
        if(!ishurt)
        {
          movement();
        }
        Jump();
        SwitchAnim();
        crough();
    }
    //蹲下狀態調整
    void crough()
    {
            if(isCrouch)
            {
                speedcoeficient = 0.6f;
                collbox.enabled = false;
            }
            else
            {
                speedcoeficient = 1.0f;
                collbox.enabled = true;
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
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        SoundManager.instance.JumpAudio();
        jumppressed = false;
        anim.SetBool("jumping",true);
       }
    }
   
    //動畫切換
    void SwitchAnim()
    {
        if(rb.velocity.y < 0.1f && !Physics2D.OverlapCircle(groundCheck.position, 1.0f, ground))
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
            }
        }
        else if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y <= 0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
        }
        
    }
    
    //物品蒐集
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag =="Collection")
        {
            Destroy(collision.gameObject);
            SoundManager.instance.CherryAudio();
            //collectAudio.Play();
            Cherry = Cherry+1;
            cherrynum.text = Cherry.ToString();
        }
        if(collision.tag =="collection diamond")
        {
            Destroy(collision.gameObject);
            SoundManager.instance.CherryAudio();
            //collectAudio.Play();
            Diamond = Diamond+1;
            dianum.text = Diamond.ToString();
        }
        if(collision.tag == "deadZone")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 1f);
            
        }
    }

    //和敵人互動
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(anim.GetBool("falling"))
            {
                enemy.explosion();    //在player程式中叫enemy程式執行指令
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 0.8f);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
                
                //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), coll);
                //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collbox);
            rb.velocity = new Vector2(-5.0f, rb.velocity.y);
            SoundManager.instance.HurtAudio();
            ishurt = true;
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
                
                
                //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), coll);
                //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collbox);
            rb.velocity = new Vector2(5.0f, rb.velocity.y);
            SoundManager.instance.HurtAudio();
            ishurt = true;
            }
        }
    }
    //場景重啟
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }
    
    

}
