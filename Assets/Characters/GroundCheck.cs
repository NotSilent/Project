using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject player;
    private new Collider collider;
    private bool isGrounded;
    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    private void Start()
    {
        player = GetPlayer();
        collider = GetComponent<Collider>();
        Physics.IgnoreCollision(collider, player.GetComponent<Collider>());

        isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }

    private GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
}