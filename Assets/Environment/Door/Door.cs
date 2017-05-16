using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    [SerializeField] private bool isTriggerOnly = false;
    [SerializeField] private new ParticleSystem particleSystem;

    private AudioSource audioSource;
    private Animator animator;

    private bool isClosed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        isClosed = true;
    }

    public void StartBeingHovered()
    {
        if (!isTriggerOnly)
        {
            particleSystem.Play();
            audioSource.Play();
        }
    }

    public void StopBeingHovered()
    {
        particleSystem.Stop();
        audioSource.Stop();
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