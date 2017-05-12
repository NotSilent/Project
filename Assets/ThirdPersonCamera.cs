using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ThirdPersonCamera : MonoBehaviour
{
    private Vector3 offset;
    private GameObject player;
    private float verticalAngle = 0;
    private float horizontalAngle = 0;

    private void Start()
    {
        GetPlayer();

        offset = transform.position - player.transform.position;

        verticalAngle = 0;
        horizontalAngle = 0;
    }

    private void GetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Assert.IsNotNull(players, "Player not found in scene. Please add one.");
        Assert.IsFalse(players.Length > 1, "Too many players in scene. Please delete all but one player.");

        player = players[0].gameObject;
    }

    private void LateUpdate()
    {
        if (verticalAngle < Input.GetAxis("Mouse Y"))
            verticalAngle += Mathf.Abs(verticalAngle - Input.GetAxis("Mouse Y"));
        else if (verticalAngle > Input.GetAxis("Mouse Y"))
            verticalAngle -= Mathf.Abs(verticalAngle - Input.GetAxis("Mouse Y"));

        if (horizontalAngle < Input.GetAxis("Mouse X"))
            horizontalAngle += Mathf.Abs(horizontalAngle - Input.GetAxis("Mouse X"));
        else if (horizontalAngle > Input.GetAxis("Mouse X"))
            horizontalAngle -= Mathf.Abs(horizontalAngle - Input.GetAxis("Mouse X"));

        transform.position = player.transform.position + offset;
        transform.Rotate(Vector3.right, verticalAngle, Space.Self);
        player.transform.Rotate(Vector3.up, horizontalAngle, Space.Self);
    }
}