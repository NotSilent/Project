using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Trigger : MonoBehaviour, IUsable
{
    [SerializeField] private GameObject triggerableObject;

    private IUsable iUsable;
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;

        iUsable = triggerableObject.GetComponent<IUsable>();
        Assert.IsNotNull(iUsable);
    }

    public void StartBeingHovered()
    {
        material.color = Color.gray;
    }

    public void StopBeingHovered()
    {
        material.color = Color.white;
    }

    public void Use(bool isTriggeredByPlayer)
    {
        iUsable.Use(false);
    }
}