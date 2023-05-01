using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    private enum CursorType
    {
        None, Attack, Hand
    }

    private CursorType cursorType = CursorType.None;
    private Texture2D attackIcon, handIcon;
    private int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);  // 레이어 마스크

    private void Start()
    {
        attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    private void Update()
    {
        UpdateMouseCursor();
    }

    /// <summary>
    /// 마우스 커서 업데이트 함수
    /// </summary>
    private void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0)) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {   // TODO: 몬스터 타겟 커서
                if (cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width / 5f, 0), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }
            }
            else
            {   // TODO: 기본 커서
                if (cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(handIcon, new Vector2(handIcon.width / 3f, 0), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }
            }
        }
    }
}