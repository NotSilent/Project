using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUsable
{
    private Material material;

    private bool isClosed;

    private void Start()
    {
        material = GetComponent<Renderer>().material;

        isClosed = false;
    }
    
    public void Use()
    {
        // TODO Animations
        if (isClosed)
        {
            transform.Translate(Vector3.right, Space.Self);
            isClosed = false;
        }
        else
        {
            transform.Translate(Vector3.left, Space.Self);
            isClosed = true;
        }
    }

    public void StartBeingHovered()
    {
        material.color = Color.yellow;
    }

    public void StopBeingHovered()
    {
        material.color = Color.green;
    }
}