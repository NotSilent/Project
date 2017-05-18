using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemiesDeadTrigger : MonoBehaviour
{
    [SerializeField] private GameObject triggerableObject;
    [SerializeField] private List<Enemy> enemies;

    private int enemyCount;

    private IUsable iUsable;

    private void OnEnable()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy)
                enemy.OnEnemyDeath += OnEnemyDeath;
        }
    }

    private void Start()
    {
        iUsable = triggerableObject.GetComponent<IUsable>();
        Assert.IsNotNull(enemies, "None enemies specified, please wire correctly");

        enemyCount = enemies.Count;
    }

    private void OnDisable()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy)
                enemy.OnEnemyDeath -= OnEnemyDeath;
        }
    }

    public void OnEnemyDeath(GameObject caller)
    {
        enemies.Remove(caller.GetComponent<Enemy>());
        enemyCount--;
        if(enemyCount == 0)
        {
            iUsable.Use(false);
        }
    }
}
