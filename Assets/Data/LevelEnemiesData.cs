using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="NewLevelEnemiesData", menuName ="LevelEnemiesData", order =2)]
public class LevelEnemiesData : ScriptableObject
{
    public enum EnemyType { archer, skeleton, demon, knight, boss}
    public List<Vector2> spawnPoints;
    public List<EnemyType> enemyTypes;



}
