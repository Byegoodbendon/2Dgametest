using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Collider2D coll;
    protected Rigidbody2D rb;
    protected AudioSource deathAudio;
    protected bool stopupdate = false;  //開始爆炸後，停止物件的更新
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        deathAudio = GetComponent<AudioSource>();
    }

    //播放爆炸動畫並同時摧毀不需要的物理性質
    public void explosion()
    {       
        anim.SetTrigger("explosioning");
        deathAudio.Play();
        stopupdate = true;
        Destroy(coll);
        Destroy(rb);

    }
    //摧毀整個物件
    public void death()
    {
        Destroy(gameObject);
    }

}
