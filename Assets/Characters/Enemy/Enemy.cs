using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    // TODO Split enemy and enemyUI
    [SerializeField] Image healthBar;
    [SerializeField] float health;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float engageRadius = 2f;

    private GameObject target;
    private Rigidbody rigidBody;
    private NavMeshAgent navMeshAgent;
    private Vector2 startingSize;
    private float currentHealth;

    public delegate void EnemyEvent(GameObject caller);
    public event EnemyEvent OnEnemyDeath;

    private void Start()
    {
        target = GameObject.FindObjectOfType<Player>().gameObject;
        Assert.IsNotNull(healthBar, "Health bar not found, please wire correctly.");

        rigidBody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        startingSize = healthBar.rectTransform.sizeDelta;
        currentHealth = health;
    }

    private void Update()
    {
            if (Vector3.Distance(transform.position, target.transform.position) < engageRadius)
            {
                rigidBody.velocity = Vector3.zero;
            }
            else if (Vector3.Distance(transform.position, target.transform.position) < patrolRadius)
            {
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(target.transform.position);
            }
            else
            {
                navMeshAgent.enabled = false;
                rigidBody.velocity = Vector3.zero;
            }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            OnEnemyDeath(gameObject);
            Destroy(gameObject);
        }

        healthBar.rectTransform.sizeDelta = new Vector2(startingSize.x * GetRelativeHealth(), startingSize.y); 
    }

    private float GetRelativeHealth()
    {
        return Mathf.Clamp(currentHealth / health, 0, 1);
    }
}