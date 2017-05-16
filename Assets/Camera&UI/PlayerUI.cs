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
    }

    // TODO Don't hardcode UI
    public void UpdateHealthBar(float relativeHealth)
    {
        healthBar.rectTransform.sizeDelta = new Vector2(healthStartingSize.x * relativeHealth, healthStartingSize.y);
    }

    public void UpdateStaminaBar(float relativeStamina)
    {
        staminaBar.rectTransform.sizeDelta = new Vector2(staminaStartingSize.x, staminaStartingSize.y * relativeStamina);
    }

    public void UpdateManaBar(float relativeMana)
    {
        Debug.Log(relativeMana);
        manaBar.rectTransform.sizeDelta = new Vector2(manaStartingSize.x, manaStartingSize.y * relativeMana);
    }
}