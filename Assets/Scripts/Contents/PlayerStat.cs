using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private UI_UnitFrame unitFrame;
    private UI_ExpBar expBar;

    private int level;
    private int exp;
    private int gold;

    public override int Hp 
    { 
        get => base.Hp;
        set
        {
            base.Hp = value;

            // TODO: �÷��̾� ü�� ���� UI �ʱ�ȭ
            if(unitFrame == null) unitFrame = Managers.UI.GetScene<UI_UnitFrame>();
            unitFrame.SetHp(Hp, MaxHp);
        }
    }

    public override int MaxHp 
    {
        get => base.MaxHp;
        set
        {
            base.MaxHp = value;

            // TODO: �÷��̾� ü�� ���� UI �ʱ�ȭ
            if (unitFrame == null) unitFrame = Managers.UI.GetScene<UI_UnitFrame>();
            unitFrame.SetHp(Hp, MaxHp);
        }
    }

    public override int Mp
    {
        get => base.Mp;
        set
        {
            base.Mp = value;

            // TODO: �÷��̾� ü�� ���� UI �ʱ�ȭ
            if (unitFrame == null) unitFrame = Managers.UI.GetScene<UI_UnitFrame>();
            unitFrame.SetMp(Mp, MaxMp);
        }
    }

    public override int MaxMp
    {
        get => base.MaxMp;
        set
        {
            base.MaxMp = value;

            // TODO: �÷��̾� ü�� ���� UI �ʱ�ȭ
            if (unitFrame == null) unitFrame = Managers.UI.GetScene<UI_UnitFrame>();
            unitFrame.SetMp(Mp, MaxMp);
        }
    }

    public int Level
    {
        get => level;
        set
        {
            level = value;

            // TODO: ���� ���� UI �ʱ�ȭ
            if (unitFrame == null) unitFrame = Managers.UI.GetScene<UI_UnitFrame>();
            unitFrame.SetLevel(Level);
        }
    }

    public int Exp 
    {
        get => exp;
        set 
        { 
            exp = value;

            // TODO: ������ üũ ����
            int level = Level;
            Data.Stat stat;
            
            while(true)
            {
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

            // TODO: ����ġ ���� UI �ʱ�ȭ
            if (expBar == null) expBar = Managers.UI.GetScene<UI_ExpBar>();

            if (!Managers.Data.StatDict.TryGetValue(level + 1, out stat))
            {
                expBar.SetExp();
            }
            else
            {
                expBar.SetExp(exp, stat.totalExp);
            }
        } 
    }

    public int Gold { get => gold; set => gold = value; }

    private void Start()
    {
        unitFrame = Managers.UI.GetScene<UI_UnitFrame>();
        expBar = Managers.UI.GetScene<UI_ExpBar>();

        Level = 1;
        defense = 5;
        moveSpeed = 5f;
        gold = 0;
        Exp = 0;
        Mp = 10;
        MaxMp = 10;

        SetStat(level);
    }

    public void SetStat(int _level)
    {
        Dictionary<int, Data.Stat> datas = Managers.Data.StatDict;
        Data.Stat stat = datas[level];

        Hp = stat.maxHp;
        MaxHp = hp;
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