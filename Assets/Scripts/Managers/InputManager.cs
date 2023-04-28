using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    private bool isPressed = false;
    private float pressedTime = 0f;

    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if(Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if(MouseAction != null)
        {
            if(Input.GetMouseButton(0))
            {
                if(!isPressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                isPressed = true;
            }
            else
            {
                if(isPressed)
                {
                    if(Time.time < pressedTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                isPressed = false;
                pressedTime = 0f;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}