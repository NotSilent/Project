using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10;

    [SerializeField] private Crosshair crosshair;

    private Rigidbody rigidBody;
    private GameObject currentTarget;

    private void OnEnable()
    {
        crosshair.OnCrosshairTargetChange += OnCrosshairTargetChange;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = transform.TransformDirection(direction);
        rigidBody.velocity = direction * speed;
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
}