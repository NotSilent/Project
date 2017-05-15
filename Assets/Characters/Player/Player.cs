using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Socket socket;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float range = 2f;
    [SerializeField] private float attackCooldown = 10f;
    [SerializeField] private float damage = 50f;

    private new Collider collider;
    private Rigidbody rigidBody;
    private GameObject currentTarget;
    private float timeSInceLastAttack;

    private void OnEnable()
    {
        crosshair.OnCrosshairTargetChange += OnCrosshairTargetChange;
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
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

        if (Input.GetMouseButton(1))
        {
            if (currentTarget)
            {
                ShootProjectile();
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
            //Debug.Log("Looking at sky");
        }
        else
        {
            //Debug.Log("Lookig at " + currentTarget.transform.name);
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

    private void ShootProjectile()
    {
        if (timeSInceLastAttack > attackCooldown)
        {
            Vector3 targetWorldSpaceCoords = GetTargetWorldSpace();
            if(targetWorldSpaceCoords == Vector3.zero)
            {
                return;
            }
            Vector3 launchDirection = targetWorldSpaceCoords - socket.transform.position;

            // Create socket in hand and launch from there
            GameObject projectile = Instantiate(projectilePrefab, socket.transform.position, Quaternion.identity);
            Physics.IgnoreCollision(collider, projectile.GetComponent<Collider>());
            projectile.GetComponent<Projectile>().LaunchProjectile(launchDirection);
            timeSInceLastAttack = 0f;
        }
    }

    // Consider refactoring Crosshair observer
    private Vector3 GetTargetWorldSpace()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            return raycastHit.point;
        }
        return Vector3.zero;
    }
}