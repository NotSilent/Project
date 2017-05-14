using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    [SerializeField] private bool isTriggerOnly = false;

    private Material material;
    private Animator animator;

    private bool isClosed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        material = GetComponent<Renderer>().material;

        isClosed = true;
    }

    public void StartBeingHovered()
    {
        material.color = Color.yellow;
    }

    public void StopBeingHovered()
    {
        material.color = Color.green;
    }

    public void Use(bool isTriggeredByPlayer)
    {
        if (isTriggeredByPlayer != isTriggerOnly || !isTriggeredByPlayer)
        {
            if (isClosed)
            {
                isClosed = false;
                animator.SetTrigger("tOpen");
            }
            else
            {
                isClosed = true;
                animator.SetTrigger("tClose");
            }
        }
    }
}