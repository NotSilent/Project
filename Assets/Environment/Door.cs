using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    [SerializeField] private bool isTriggerOnly = false;
    [SerializeField] private new ParticleSystem particleSystem;
    
    private Animator animator;

    private bool isClosed;

    private void Start()
    {
        animator = GetComponent<Animator>();

        isClosed = true;
    }

    public void StartBeingHovered()
    {
        particleSystem.Play();
    }

    public void StopBeingHovered()
    {
        particleSystem.Stop();
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