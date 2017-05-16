using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Trigger : MonoBehaviour, IUsable
{
    [SerializeField] private GameObject triggerableObject;
    [SerializeField] private new ParticleSystem particleSystem;

    private IUsable iUsable;
    private AudioSource audioSource;
    private Animator animator;

    private bool isClosed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        iUsable = triggerableObject.GetComponent<IUsable>();
        Assert.IsNotNull(iUsable);

        isClosed = true;
    }

    public void StartBeingHovered()
    {
        particleSystem.Play();
        audioSource.Play();
    }

    public void StopBeingHovered()
    {
        particleSystem.Stop();
        audioSource.Stop();
    }

    public void Use(bool isTriggeredByPlayer)
    {
        if (isClosed)
        {
            isClosed = false;
            animator.SetTrigger("tOn");
        }
        else
        {
            isClosed = true;
            animator.SetTrigger("tOff");
        }

        iUsable.Use(false);
    }
}