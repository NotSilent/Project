using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ThirdPersonCamera : MonoBehaviour
{
    private Vector3 offset;
    private GameObject player;

    private void Start()
    {
        player = GetPlayer();

        offset = transform.position - player.transform.position;
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
        gameObject.transform.position = player.transform.position + offset;
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"), Space.Self);
        player.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.Self);

        //TODO Possibly forbid player too look to high/low
    }
}