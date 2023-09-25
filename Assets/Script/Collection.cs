using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public Animator anim;
    public void CherryGet()
    {
        FindObjectOfType<Player>().CherryCount();
    }
    public void DiamondGet()
    {
        FindObjectOfType<Player>().DiamondCount();
    }
    
    public void CollectionDestroy()
    {
        Destroy(gameObject);
    }
}
