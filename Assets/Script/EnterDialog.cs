using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{
    public GameObject entertext;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            entertext.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player")
        {

            entertext.SetActive(false);

        }
    }
}
