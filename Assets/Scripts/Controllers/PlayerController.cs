using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die, Idle, Moving, Attack
    }

    private enum CursorType
    {
        None, Attack, Hand
    }

    private CursorType cursorType = CursorType.None;
    private Texture2D attackIcon, handIcon;

    private PlayerStat stat;
    private Vector3 destPos;            // 목표지점
    private PlayerState state = PlayerState.Idle;

    private void Start()
    {
        attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private void Update()
    {
        UpdateMouseCursor();

        switch (state)
        {
            case PlayerState.Die: UpdateDie(); break;
            case PlayerState.Idle: UpdateIdle(); break;
            case PlayerState.Moving: UpdateMoving(); break;
        }
    }

    /// <summary>
    /// 마우스 커서 업데이트 함수
    /// </summary>
    private void UpdateMouseCursor()
    {
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

    private void UpdateDie()
    {

    }

    private void UpdateIdle()
    {
        // TODO: 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    /// <summary>
    /// 플레이어 상태가 Moving일 경우 실행되는 업데이트 함수
    /// </summary>
    private void UpdateMoving()
    {
        Vector3 dir = destPos - transform.position;     // 목표지점의 방향벡터
        if (dir.magnitude < 0.1f) state = PlayerState.Idle;
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.red);
            if(Physics.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Block")))
            {
                state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        // TODO: 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", stat.MoveSpeed);
    }

    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);  // 레이어 마스크
    /// <summary>
    /// 마우스 입력 움직임 제어 함수
    /// </summary>
    /// <param name="_evt">마우스 이벤트 종류</param>
    private void OnMouseClicked(Define.MouseEvent _evt)
    {
        if (state == PlayerState.Die) return;
        if (_evt != Define.MouseEvent.Click) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100f, mask))
        {
            destPos = hit.point;
            state = PlayerState.Moving;

            if(hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {   // TODO: 몬스터 클릭 이벤트
                Debug.Log("몬스터 클릭");
            }
            else
            {   // TODO: 지형 클릭 이벤트
                Debug.Log("지형 클릭");
            }
        }
    }

    #region Event Callback
    private void OnRunEvent()
    {
        Debug.Log("터벅");
    }
    #endregion
}