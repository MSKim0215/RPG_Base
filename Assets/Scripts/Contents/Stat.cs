using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat: MonoBehaviour
{
    protected int hp;
    protected int maxHp;
    protected int mp;
    protected int maxMp;
    protected int attack;
    protected int defense;
    protected float moveSpeed;

    public virtual int Hp { get { return hp; } set { hp = value; } }
    public virtual int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public virtual int Mp { get { return mp; } set { mp = value; } }
    public virtual int MaxMp { get { return maxMp; } set { maxMp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    private void Start()
    {
        ClearStat();
    }

    public void ClearStat()
    {
        hp = 100;
        maxHp = hp;
        mp = 10;
        maxMp = mp;
        attack = 10;
        defense = 5;
        moveSpeed = 5f;
    }

    /// <summary>
    /// 피해 함수
    /// </summary>
    /// <param name="_attacker">공격자 스탯</param>
    public virtual void OnAttacked(Stat _attacker)
    {
        int damage = Mathf.Max(0, _attacker.attack - Defense);
        Hp -= damage;
        if(Hp <= 0)
        {
            Hp = 0;
            OnDead(_attacker);
        }
    }

    /// <summary>
    /// 죽음 함수
    /// </summary>
    protected virtual void OnDead(Stat _attacker)
    {
        PlayerStat playerStat = _attacker as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Exp += 15;
        }
        Managers.Game.Despawn(gameObject);
    }
}