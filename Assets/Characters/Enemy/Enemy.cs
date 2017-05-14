using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] Image healthBar;
    [SerializeField] float health;

    private Vector2 startingSize;
    private float currentHealth;

    private void Start()
    {
        Assert.IsNotNull(healthBar, "Health bar not found, please wire correctly.");

        startingSize = healthBar.rectTransform.sizeDelta;
        currentHealth = health;
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        healthBar.rectTransform.sizeDelta = new Vector2(startingSize.x * GetRelativeHealth(), startingSize.y); 
    }

    private float GetRelativeHealth()
    {
        return Mathf.Clamp(currentHealth / health, 0, 1);
    }
}