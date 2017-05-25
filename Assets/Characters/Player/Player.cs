using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Socket weaponSocket;
    [SerializeField] private Socket groundCheck;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float range = 2f;
    [SerializeField] private float attackCooldown = 10f;
    [SerializeField] private float damage = 50f;

    [System.Serializable]
    public struct Resources
    {
        public float health;
        public float healthRegeneration;
        public float stamina;
        public float staminaRegeneration;
        public float basicStaminaCost;
        public float mana;
        public float manaRegeneration;
    }

    [SerializeField]
    private Resources resources;

    private new Collider collider;
    private Rigidbody rigidBody;
    private GameObject currentTarget;
    private float timeSinceLastAttack;
    private float currentHealth;
    private float currentStamina;
    private float currentMana;

    private void OnEnable()
    {
        crosshair.OnCrosshairTargetChange += OnCrosshairTargetChange;
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();

        timeSinceLastAttack = attackCooldown;
        currentHealth = resources.health;
        currentStamina = resources.stamina;
        currentMana = resources.mana;
    }

    private void Update()
    {
        RegenerateResources();

        timeSinceLastAttack += Time.deltaTime;
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
        Vector3 direction = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        direction = transform.TransformDirection(direction);

        if (Input.GetKeyDown(KeyCode.Space) && IsPlayerGrounded())
        {
            rigidBody.velocity += Vector3.up * jumpPower;
        }
        
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
            if (timeSinceLastAttack > attackCooldown && currentStamina > resources.basicStaminaCost)
            {
                float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, currentTarget.transform.position);
                if (distanceFromPlayer < range)
                {
                    iDamageable.TakeDamage(damage);
                    timeSinceLastAttack = 0f;
                    currentStamina -= resources.basicStaminaCost;
                    playerUI.UpdateStaminaBar(GetRelativeStamina());
                }
            }
        }
    }

    private void ShootProjectile()
    {
        if (timeSinceLastAttack > attackCooldown && currentMana > projectilePrefab.GetComponent<Projectile>().ManaCost)
        {
            Vector3 targetWorldSpaceCoords = GetTargetWorldSpace();
            if (targetWorldSpaceCoords == Vector3.zero)
            {
                return;
            }
            Vector3 launchDirection = targetWorldSpaceCoords - weaponSocket.transform.position;

            // Create socket in hand and launch from there
            GameObject projectileObject = Instantiate(projectilePrefab, weaponSocket.transform.position, Quaternion.identity);
            Physics.IgnoreCollision(collider, projectileObject.GetComponent<Collider>());
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.LaunchProjectile(launchDirection);
            currentMana -= projectile.ManaCost;
            playerUI.UpdateManaBar(GetRelativeMana());
            timeSinceLastAttack = 0f;
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

    private void RegenerateResources()
    {
        if (currentHealth < resources.health)
        {
            currentHealth += resources.healthRegeneration * Time.deltaTime;
            playerUI.UpdateHealthBar(GetRelativeHealth());
        }
        if (currentStamina < resources.stamina)
        {
            currentStamina += resources.staminaRegeneration * Time.deltaTime;
            playerUI.UpdateStaminaBar(GetRelativeStamina());
        }
        if (currentMana < resources.mana)
        {
            currentMana += resources.manaRegeneration * Time.deltaTime;
            playerUI.UpdateManaBar(GetRelativeMana());
        }
    }

    private float GetRelativeHealth()
    {
        return Mathf.Clamp(currentHealth / resources.health, 0, 1);
    }

    private float GetRelativeStamina()
    {
        return Mathf.Clamp(currentStamina / resources.stamina, 0, 1);
    }

    private float GetRelativeMana()
    {
        return Mathf.Clamp(currentMana / resources.mana, 0, 1);
    }

    private bool IsPlayerGrounded()
    {
        Collider[] colliders = Physics.OverlapBox(groundCheck.transform.position, Vector3.one * 0.1f);

        if (colliders.Length != 1 && colliders[0] != collider)
        {
            return true;
        }
        return false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // TODO Possible OnPlayerDeathEvent
            Camera.main.transform.parent = transform.parent;
            Destroy(gameObject);
        }

        playerUI.UpdateHealthBar(GetRelativeHealth());
    }
}