using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ThirdPersonCamera : MonoBehaviour
{
    private Vector3 offset;
    private GameObject player;

    private float distanceFromCamera;

    // TODO separate from player
    private const float playerScale = 5;

    private const float distanceFromWallRatio = 1.1f;

    private float cameraDistanceScale;

    private void Start()
    {
        Cursor.visible = false;
        player = GetPlayer();
        offset = Camera.main.transform.position - transform.position;
        distanceFromCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        cameraDistanceScale = playerScale * distanceFromWallRatio;
    }

    private GameObject GetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Assert.IsNotNull(players, "Player not found in scene. Please add one.");
        Assert.IsFalse(players.Length > 1, "Too many players in scene. Please delete all but one player.");

        return players[0].gameObject;
    }

    private void LateUpdate()
    {
        Camera.main.transform.localPosition = offset / cameraDistanceScale;
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"), Space.Self);
        player.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.Self);

        RaycastHit raycastHit;
        if(Physics.Raycast(transform.position, Camera.main.transform.position, out raycastHit, distanceFromCamera))
        {
            Vector3 cameraLocalPosition = Camera.main.transform.localPosition;
            Camera.main.transform.localPosition = new Vector3(cameraLocalPosition.x, cameraLocalPosition.y, -raycastHit.distance / cameraDistanceScale);
            print(raycastHit.distance);
        }

        //TODO Possibly forbid player too look to high/low
    }
}