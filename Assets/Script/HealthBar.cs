using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health playerHealth;
    public Image totalHealthBar;
    public Image currentHealthBar;

    void Start()
    {
        totalHealthBar.fillAmount = (float)playerHealth.currentHealth/10;  //(float)後面放整數的話，可以變成浮點數
    }

    void Update()
    {
        currentHealthBar.fillAmount = (float)playerHealth.currentHealth/10;   
    }
}
