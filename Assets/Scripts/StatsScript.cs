using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    int health, damage, strength, defense, durability, ammo, gold, experience, level; //damage is total strength, defense is total durability
    public int expGain, expLvRequired;

    int[] currentUpgrade = new int[4]; //0 = weapon, 1 = armor
    float moveSpeed;

    bool[] hasBossKeys = new bool [3]; //three main areas
    bool[] hasKeys;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int Damage
    {
        get
        {
            damage = strength + currentUpgrade[0];
            return damage;
        }
        set
        {
            strength = value;
        }
    }

    public int Defense
    {
        get
        {
            defense = durability + currentUpgrade[1];
            return defense;
        }
        set
        {
            durability = value;
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
        set
        {
            moveSpeed = value;
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

    public int Experience
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

    void GainExp()
    {

        if (experience >= expLvRequired) //if player has 107 experience, and only needed 100, save the 7
        {
            experience -= experience;
            LevelUp();
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
            moveSpeed += .01f;
        }
    }

    void AddGold(int goldAmount)
    {
        gold += goldAmount;
    }
}
