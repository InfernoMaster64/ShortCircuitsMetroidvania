using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    public int maxHealth;
    public float attackCooldown;
    public float attackRange;
    public float speed;
    public int damage;
    public RuntimeAnimatorController animControl;
    
}
