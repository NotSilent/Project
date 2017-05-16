using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float manaCost = 50f;

    private new Collider collider;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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