using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChangeLevelTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private string sceneName;

    private GameObject player;

    private void Start()
    {
        player = GetPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            levelManager.ChangeScene(sceneName);
        }
    }

    private GameObject GetPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Assert.IsNotNull(players, "Player not found in scene. Please add one.");
        Assert.IsFalse(players.Length > 1, "Too many players in scene. Please delete all but one player.");

        return players[0].gameObject;
    }
}
