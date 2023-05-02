using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private int exp;
    private int gold;

    public int Exp 
    { 
        get { return exp; } 
        set 
        { 
            exp = value;

            // TODO: ������ üũ ����
            int level = Level;
            while(true)
            {
                Data.Stat stat;
                if (!Managers.Data.StatDict.TryGetValue(level + 1, out stat)) break;
                if (exp < stat.totalExp) break;
                level++;
            }

            if(level != Level)
            {
                Debug.Log("������!!!!!!!!!!!!");
                Level = level;
                SetStat(Level);
            }
        } 
    }
    public int Gold { get { return gold; } set { gold = value; } }

    private void Start()
    {
        level = 1;
        defense = 5;
        moveSpeed = 5f;
        gold = 0;
        exp = 0;

        SetStat(level);
    }

    public void SetStat(int _level)
    {
        Dictionary<int, Data.Stat> datas = Managers.Data.StatDict;
        Data.Stat stat = datas[level];

        hp = stat.maxHp;
        maxHp = hp;
        attack = stat.attack;
    }

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    protected override void OnDead(Stat _attacker)
    {
        Debug.Log("Player Dead");
    }
}