using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth = 3;
    public Menu menu;
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    public void Hurt()
    {
        if(currentHealth > 0)
        {
            currentHealth -= 1;
        }else{
            currentHealth = 0;
        }
        if(currentHealth == 0)
        {
            Time.timeScale = 0.2f;
            Invoke("gameOver",0.3f);
        }
    }
    public void gameOver()
    {
        menu.GameOver();
    }
    public void Tes1t()
    {
        Debug.Log("2");
    }

}
