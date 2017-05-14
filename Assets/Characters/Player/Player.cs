using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float range = 2f;
    [SerializeField] private float attackCooldown = 10f;
    [SerializeField] private float damage = 50f;

    private Rigidbody rigidBody;
    private GameObject currentTarget;
    private float timeSInceLastAttack;

    private void OnEnable()
    {
        crosshair.OnCrosshairTargetChange += OnCrosshairTargetChange;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        timeSInceLastAttack = attackCooldown;
    }

    private void Update()
    {
        timeSInceLastAttack += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if (currentTarget)
            {
                ManageDamageableTarget();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget)
            {
                ManageUsableTarget();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = transform.TransformDirection(direction);
        rigidBody.velocity = direction * speed + new Vector3(0, rigidBody.velocity.y, 0);
    }

    private void OnDisable()
    {
        crosshair.OnCrosshairTargetChange -= OnCrosshairTargetChange;
    }

    void OnCrosshairTargetChange(GameObject target)
    {
        currentTarget = target;

        if (!currentTarget)
        {
            Debug.Log("Looking at sky");
        }
        else
        {
            Debug.Log("Lookig at " + currentTarget.transform.name);
        }
    }


    private void ManageUsableTarget()
    {
        IUsable iUsable = currentTarget.GetComponent<IUsable>();
        if (iUsable != null)
        {

            float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, currentTarget.transform.position);
            if (distanceFromPlayer < range)
            {
                iUsable.Use(true);
            }
        }
    }

    private void ManageDamageableTarget()
    {
        IDamageable iDamageable = currentTarget.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            if (timeSInceLastAttack > attackCooldown)
            {
                float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, currentTarget.transform.position);
                if (distanceFromPlayer < range)
                {
                    iDamageable.DealDamage(50f);
                    timeSInceLastAttack = 0f;
                }
            }
        }
    }
}