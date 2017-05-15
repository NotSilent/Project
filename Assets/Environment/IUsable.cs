using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    // TODO Consider spliting
    void Use(bool isTriggeredByPlayer);

    void StartBeingHovered();

    void StopBeingHovered();
}