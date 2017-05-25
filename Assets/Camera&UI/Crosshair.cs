using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private new Camera camera;
    private Image crosshairImage;
    private GameObject crosshairTarget;
    private GameObject player;

    public delegate void CrosshairEvent(GameObject crosshairTarget);
    public event CrosshairEvent OnCrosshairTargetChange;

    private void Start()
    {
        camera = Camera.main;
        crosshairImage = GetComponent<Image>();
        crosshairTarget = null;
        player = GetPlayer();
    }

    private void Update()
    {
        GameObject currentTarget = GetCrosshairTarget();

        if (currentTarget != crosshairTarget)
        {
            StopPreviousTargetInteractions();

            crosshairTarget = currentTarget;

            if (OnCrosshairTargetChange != null)
            {
                OnCrosshairTargetChange(crosshairTarget);
            }

            StartNewTargetInteractions();
        }
    }

    private void StartNewTargetInteractions()
    {
        if (!crosshairTarget)
        {
            crosshairImage.color = Color.white;
        }
        else
        {
            if (crosshairTarget.GetComponent<IDamageable>() != null)
            {
                crosshairImage.color = Color.red;
            }
            else
            {
                crosshairImage.color = Color.white;
            }

            IUsable iUsable = crosshairTarget.GetComponent<IUsable>();
            if (iUsable != null)
            {
                iUsable.StartBeingHovered();
            }
        }
    }

    private void StopPreviousTargetInteractions()
    {
        if (crosshairTarget)
        {
            IUsable iUsable = crosshairTarget.GetComponent<IUsable>();
            if (iUsable != null)
            {
                iUsable.StopBeingHovered();
            }
        }
    }

    private GameObject GetCrosshairTarget()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if(raycastHit.transform.gameObject != player)
            return raycastHit.transform.gameObject;
        }
        return null;
    }

    private GameObject GetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Assert.IsNotNull(players, "Player not found in scene. Please add one.");
        Assert.IsFalse(players.Length > 1, "Too many players in scene. Please delete all but one player.");

        return players[0].gameObject;
    }
}