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

    public void Hurt(int damage)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damage;

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
        SoundManager.instance.DeathAudio();
    }

}
