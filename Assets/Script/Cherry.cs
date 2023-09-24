using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cherry : MonoBehaviour
{
    public Animator anim;
    public void CherryGet()
    {
        FindObjectOfType<Player>().CherryCount();
    }
    
    public void CherryDestroy()
    {
        Destroy(gameObject);
    }
}
