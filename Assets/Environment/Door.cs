using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IHoverable
{
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
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