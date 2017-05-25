using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image manaBar;

    private Vector2 healthStartingSize;
    private Vector2 staminaStartingSize;
    private Vector2 manaStartingSize;

    private void Start()
    {
        healthStartingSize = healthBar.rectTransform.sizeDelta;
        staminaStartingSize = staminaBar.rectTransform.sizeDelta;
        manaStartingSize = manaBar.rectTransform.sizeDelta;
        
        staminaBar.color = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, 0);
        manaBar.color = new Color(manaBar.color.r, manaBar.color.g, manaBar.color.b, 0);
    }
    
    public void UpdateHealthBar(float relativeHealth)
    {
        healthBar.rectTransform.sizeDelta = new Vector2(healthStartingSize.x * relativeHealth, healthStartingSize.y);
    }

    public void UpdateStaminaBar(float relativeStamina)
    {
        staminaBar.rectTransform.sizeDelta = new Vector2(staminaStartingSize.x, staminaStartingSize.y * relativeStamina);

        if (relativeStamina < 1)
        {
            staminaBar.color = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, 255);
        }
        else
        {
            staminaBar.color = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, 0);
        }
    }

    public void UpdateManaBar(float relativeMana)
    {
        manaBar.rectTransform.sizeDelta = new Vector2(manaStartingSize.x, manaStartingSize.y * relativeMana);

        if (relativeMana < 1)
        {
            manaBar.color = new Color(manaBar.color.r, manaBar.color.g, manaBar.color.b, 255);
        }
        else
        {
            manaBar.color = new Color(manaBar.color.r, manaBar.color.g, manaBar.color.b, 0);
        }
    }
}