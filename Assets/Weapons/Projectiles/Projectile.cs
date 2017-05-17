using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float manaCost = 50f;
    [SerializeField] private float lifetime = 5f;

    private new Collider collider;
    private Rigidbody rigidBody;
    private float currentLifetime;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        currentLifetime = 0f;
    }

    private void Update()
    {
        currentLifetime += Time.deltaTime;

        if(lifetime <= currentLifetime)
        {
            Destroy(gameObject);
        }
    }

    public void LaunchProjectile(Vector3 launchDirection)
    {
        rigidBody.velocity = launchDirection.normalized * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
        }
        Debug.Log(collision.gameObject.transform.name);
        Destroy(gameObject);
    }

    public float ManaCost
    {
        get { return manaCost; }
    }
}