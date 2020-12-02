using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    private static StatsScript instance;

    int health, damage, strength, defense, durability, ammo, gold, experience, level; //damage is total strength, defense is total durability
    public int expGain, expLvRequired;

    int[] currentUpgrade = new int[4]; //0 = weapon, 1 = armor
    float moveSpeed;

    bool[] hasBossKeys = new bool [3]; //three main areas
    bool[] hasKeys;

    private void Awake()
    {
        if (instance == null) //prevents duplicate stat objects
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Object.Destroy(gameObject);
        }

        health = 100;
        strength = 10;
        durability = 10;
        moveSpeed = 6;
        ammo = 20;
    }

    public int Health
    {
        get
        {
            return health;
        }
    }

    public int Damage
    {
        get
        {
            damage = strength + currentUpgrade[0];
            return damage;
        }
    }

    public int Defense
    {
        get
        {
            defense = durability + currentUpgrade[1];
            return defense;
        }
    }

    public int Ammo
    {
        get
        {
            return ammo;
        }
        set
        {
            ammo = value;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    }

    public int EXP
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    public void GainExp(int exp)
    {
        experience += exp;

        if (experience >= expLvRequired) //if player has 107 experience, and only needed 100, save the 7
        {
            experience -= expLvRequired;
            LevelUp();
        }
        else
        {
            Debug.Log("Experience: " + experience + ", required for next level: " + expLvRequired);
        }
    }

    void LevelUp()
    {
        health += 10;
        strength++;
        durability++;
        level++;
        expLvRequired += 100;

        if (level%2 == 0)
        {
            moveSpeed += .5f;
        }
        Debug.Log("Level up! Level " + level);
        Debug.Log("Health: " + health);
        Debug.Log("Strength: " + strength);
        Debug.Log("Durability: " + durability);
        Debug.Log("Exp to next: " + expLvRequired);
        Debug.Log("Remaining experience: " + experience);
    }

    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
    }
}
