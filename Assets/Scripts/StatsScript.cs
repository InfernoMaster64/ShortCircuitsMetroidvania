using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    int health, damage, ammo, defense, gold, experience, level;

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
            return damage;
        }
        set
        {
            damage = value;
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
}
