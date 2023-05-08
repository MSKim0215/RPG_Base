using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Joystick : UI_Scene
{
    private enum GameObjects
    {
        FixedJoystick
    }

    private FixedJoystick joystick;

    public FixedJoystick Joystick 
    { 
        get
        {
            if (joystick == null)
            {
                joystick = GetComponentInChildren<FixedJoystick>();
            }
            return joystick;
        }
    }

    public override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        
        joystick = GetComponentInChildren<FixedJoystick>();
    }
}