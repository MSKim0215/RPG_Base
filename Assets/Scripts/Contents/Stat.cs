using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat: MonoBehaviour
{
    protected int hp;
    protected int maxHp;
    protected int attack;
    protected int defense;
    protected float moveSpeed;

    public int Hp { get { return hp; } set { hp = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
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
        attack = 10;
        defense = 5;
        moveSpeed = 5f;
    }

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    /// <param name="_attacker">������ ����</param>
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
    /// ���� �Լ�
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