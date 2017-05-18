using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    [SerializeField] private bool isTriggerOnly = false;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip deny;

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
            audioSource.clip = hover;
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
        else
        {
            audioSource.clip = deny;
            audioSource.Play();
        }
    }
}