using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Trigger : MonoBehaviour, IUsable
{
    [System.Serializable]
    struct Parameters
    {
        [Tooltip("Delay on trigger use")]
        public float delayUse;
        [Tooltip("How long before trigger turns off")]
        public float triggerTime;
    }

    [SerializeField] private GameObject[] triggerableObjects;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private Parameters parameters;

    private List<IUsable> iUsables = new List<IUsable>();
    private AudioSource audioSource;
    private Animator animator;

    private bool isClosed;
    private bool isBeingUsed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        foreach (GameObject triggerableObject in triggerableObjects)
            iUsables.Add(triggerableObject.GetComponent<IUsable>());

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
        if (!isBeingUsed)
        {
            AnimateTrigger();

            if (parameters.delayUse > 0)
            {
                StartCoroutine(UseDelayed(parameters.delayUse));
            }
            else
            {
                StartCoroutine(UseDelayed(0));
            }
        }
    }

    IEnumerator UseDelayed(float time)
    {
        isBeingUsed = true;
        yield return new WaitForSeconds(time);
        foreach (IUsable iUsable in iUsables)
            iUsable.Use(false);

        if (parameters.triggerTime > 0)
        {
            StartCoroutine(Close(parameters.triggerTime));

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
        }
        isBeingUsed = false;
    }

    IEnumerator Close(float time)
    {
        yield return new WaitForSeconds(time);
        isClosed = true;
        animator.SetTrigger("tOff");

        foreach (IUsable iUsable in iUsables)
            iUsable.Use(false);
    }

    private void AnimateTrigger()
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
    }
}