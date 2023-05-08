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
    }

    private void OnAttack(PointerEventData _data)
    {
        if(player.State != Define.CharacterState.Attack)
        {
            player.ScanTarget();
        }
        else
        {
            player.ResetTarget();
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