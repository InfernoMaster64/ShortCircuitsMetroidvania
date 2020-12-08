using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public LevelEnemiesData castleEnemies;
    public EnemyData skeleton;
    public EnemyData archer;



    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < castleEnemies.spawnPoints.Count; i++)
        {
            GameObject enemy = new GameObject();
            enemy.transform.position = castleEnemies.spawnPoints[i];
            EnemyController enemyController = enemy.AddComponent<EnemyController>();
            if(i >= castleEnemies.enemyTypes.Count)
            {
                Debug.LogError("List of enemy types too short in CastleData");
            }
            else if (castleEnemies.enemyTypes[i] == LevelEnemiesData.EnemyType.skeleton)
            {
                enemyController.enemyData = skeleton;
            }
            else if (castleEnemies.enemyTypes[i] == LevelEnemiesData.EnemyType.archer)
            {
                enemyController.enemyData = archer;
            }
            else
            {
                Debug.LogError("Issue with declaring the enemyType to new enemy_" + i + ". EnemyType not reconized.");
            }
        }
    }


}
