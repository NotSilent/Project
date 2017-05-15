using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class VolumeTrigger : MonoBehaviour, IUsable
{
    [SerializeField] private GameObject triggerableObject;
    [SerializeField] private bool isOneTimeOnly = true;

    private IUsable iUsable;
    private bool wasUsed;

    private void Start()
    {
        iUsable = triggerableObject.GetComponent<IUsable>();
        Assert.IsNotNull(iUsable);
        wasUsed = false;
    }

#region Not Applicable
    public void StartBeingHovered()
    {
    }

    public void StopBeingHovered()
    {
    }
#endregion

    public void Use(bool isTriggeredByPlayer)
    {
        if(isOneTimeOnly)
        {
            if (!wasUsed)
            {
                iUsable.Use(false);
                wasUsed = true;
            }
        } else
        {
            iUsable.Use(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO Consider allowing designer to specify on which layers will trigger fire
        if(other.gameObject.GetComponent<Player>())
        {
            Use(true);
        }
    }
}