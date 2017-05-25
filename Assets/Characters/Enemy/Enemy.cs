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
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float damage = 10f;

    private GameObject currentTarget;
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;
    private Vector2 startingUISize;
    private float currentHealth;
    private float timeSinceLastAttack;

    public delegate void EnemyEvent(GameObject caller);
    public event EnemyEvent OnEnemyDeath;

    private void Start()
    {
        currentTarget = GameObject.FindObjectOfType<Player>().gameObject;
        Assert.IsNotNull(healthBar, "Health bar not found, please wire correctly.");

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        startingUISize = healthBar.rectTransform.sizeDelta;
        currentHealth = health;

        timeSinceLastAttack = 0f;
    }

    private void Update()
    {
        if (currentTarget)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (Vector3.Distance(transform.position, currentTarget.transform.position) < engageRadius)
            {
                rigidBody.velocity = Vector3.zero;
                ManageDamageableTarget();
            }
            else if (Vector3.Distance(transform.position, currentTarget.transform.position) < patrolRadius)
            {
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
            }
            else
            {
                navMeshAgent.enabled = false;
                rigidBody.velocity = Vector3.zero;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (currentHealth <= 0)
        {
            OnEnemyDeath(gameObject);
            Destroy(gameObject);
        }

        healthBar.rectTransform.sizeDelta = new Vector2(startingUISize.x * GetRelativeHealth(), startingUISize.y);
    }

    private float GetRelativeHealth()
    {
        return Mathf.Clamp(currentHealth / health, 0, 1);
    }

    private void ManageDamageableTarget()
    {
        IDamageable iDamageable = currentTarget.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            if (timeSinceLastAttack > attackCooldown)
            {
                iDamageable.TakeDamage(damage);
                timeSinceLastAttack = 0f;
            }
        }
    }
}