using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceCameraController : MonoBehaviour
{
    private enum ViewMode
    {
        None, Origin, Attack
    }

    private PlayerController player;
    private Vector3 originPos;
    private Vector3 attackPos;
    private ViewMode viewMode;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
        originPos = transform.localPosition;
        attackPos = new Vector3(originPos.x, 1.24f, originPos.z);
        viewMode = ViewMode.None;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            if (player.State != Define.CharacterState.Attack)
            {
                if(viewMode != ViewMode.Origin)
                {
                    viewMode = ViewMode.Origin;
                    transform.localPosition = originPos;
                }
            }
            else
            {
                if(viewMode != ViewMode.Attack)
                {
                    viewMode = ViewMode.Attack;
                    transform.localPosition = attackPos;
                }
            }
        }
    }
}