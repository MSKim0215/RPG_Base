using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AttackButton : UI_Scene
{
    private enum Buttons
    {
        Btn_Attack,
        Btn_Auto
    }

    private PlayerController player;

    public void SetPlayer(GameObject _player) => player = _player.GetComponent<PlayerController>();

    public override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();

        Bind<Button>(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Btn_Attack).gameObject, OnAttack);
        BindEvent(GetButton((int)Buttons.Btn_Auto).gameObject, OnAuto);
    }

    private void OnAttack(PointerEventData _data)
    {
        if (player.autoAttack) return;

        if(player.State != Define.CharacterState.Attack)
        {
            player.ScanTarget();
        }
        else
        {
            player.ResetTarget();
        }
    }

    private void OnAuto(PointerEventData _data)
    {
        if (player.autoAttack)
        {
            player.ResetTarget();
            player.autoAttack = false;
            SetAttackButtonColor(Color.white);
            SetAutoButtonColor(Color.white);
        }
        else
        {
            player.ScanTarget();
            player.autoAttack = true;
            SetAttackButtonColor(Color.yellow);
            SetAutoButtonColor(Color.yellow);
        }
    }

    public void SetAttackButtonColor(Color _color)
    {
        GetButton((int)Buttons.Btn_Attack).image.color = _color;
    }

    public void SetAutoButtonColor(Color _color)
    {
        GetButton((int)Buttons.Btn_Auto).image.color = _color;
    }
}