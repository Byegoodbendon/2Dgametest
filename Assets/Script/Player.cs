using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Sprites;

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
    private float speedcoeficient = 1.0f;   //站立與蹲下速度參數
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
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 2f;
    private float HurtTime;   //受傷計時器
    void Start()
    {
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
       if(Input.GetButtonDown("Jump") && pcoll.IsTouchingLayers(ground))//Physics2D.OverlapCircle(groundCheck.position, 1.0f, ground))
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
        cherrynum.text = Cherry.ToString();    //顯示得到櫻桃的數量
        dianum.text = Diamond.ToString();  //顯示得到鑽石的數量
      
    }

    void FixedUpdate()
    {
        if(!ishurt)      //若受傷狀態為True則無法操控角色
        {
            gameObject.layer = 3;   //沒受傷的話保持角色Layer為Player
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
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) ;//* Time.deltaTime;

        }
        SoundManager.instance.JumpAudio();
        jumppressed = false;
        anim.SetBool("jumping",true);
       }
    }
   
    //動畫切換
    void SwitchAnim()
    {
        if(rb.velocity.y < 0.1f && !pcoll.IsTouchingLayers(ground))  //用腳底的collider確認有沒有踩到地板
        {
            anim.SetBool("falling",true);
        }
        if(ishurt)
        {
            anim.SetBool("hurting",true);
            anim.SetFloat("running",0);
            DelayTime();
        }
        else if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y <= 0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
            else
            {
                anim.SetBool("jumping",true);
                anim.SetBool("falling",false);
            }
        }
        else if(pcoll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
        }
        
    }
    //受傷延時
    void DelayTime()
    {
        HurtTime += Time.deltaTime;
        if(HurtTime > 0.7f)                         //受傷時無法操控角色的時間
        {
            HurtTime = 0f;
            ishurt = false;
            anim.SetBool("hurting",false);
        }
    }
    
    //物品蒐集
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag =="Collection")
        {
            Collection cherry = collision.gameObject.GetComponent<Collection>();
            Destroy(cherry.GetComponent<Collider2D>());
            SoundManager.instance.CherryAudio();
            cherry.anim.SetTrigger("getting");
        }
        if(collision.tag =="collection diamond")
        {
            Collection diamond = collision.gameObject.GetComponent<Collection>();
            Destroy(diamond.GetComponent<Collider2D>());
            SoundManager.instance.DiamondAudio();
            diamond.anim.SetTrigger("getting");
        }
        if(collision.tag == "deadZone")
        {
            Health health = GetComponent<Health>();
            GetComponent<AudioSource>().enabled = false;
            health.Hurt(3);
        }
    }

    //和敵人互動
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Health health = GetComponent<Health>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(anim.GetBool("falling"))
            {
                enemy.explosion();    //在player程式中叫enemy程式執行指令
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * 0.8f);
            }
            else if(transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5.0f, rb.velocity.y);
                gameObject.layer = 7;     //將角色的layer設為序號7的enemy
                SoundManager.instance.HurtAudio();
                ishurt = true;
                health.Hurt(1);
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5.0f, rb.velocity.y);
                gameObject.layer = 7;     //將角色的layer設為序號7的enemy
                SoundManager.instance.HurtAudio();
                ishurt = true;
                health.Hurt(1);
            }
        }
    }
    //場景重啟
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //櫻桃計算
    public void CherryCount()
    {
        Cherry += 1;   
    }
    //鑽石計算
    public void DiamondCount()
    {
        Diamond += 1;
    }

}
