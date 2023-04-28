using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private int exp;
    private int gold;

    public int Exp { get { return exp; } set { exp = value; } }
    public int Gold { get { return gold; } set { gold = value; } }

    private void Start()
    {
        level = 1;
        hp = 100;
        maxHp = hp;
        attack = 10;
        defense = 5;
        moveSpeed = 5f;
        exp = 0;
        gold = 0;
    }
}