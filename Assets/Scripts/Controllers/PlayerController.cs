using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die, Idle, Moving
    }

    private float speed = 10f;
    private Vector3 destPos;            // 목표지점
    private PlayerState state = PlayerState.Idle;

    private void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private void Update()
    {
        switch(state)
        {
            case PlayerState.Die: UpdateDie(); break;
            case PlayerState.Idle: UpdateIdle(); break;
            case PlayerState.Moving: UpdateMoving(); break;
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
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
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
        anim.SetFloat("speed", speed);
    }

    /// <summary>
    /// 마우스 입력 움직임 제어 함수
    /// </summary>
    /// <param name="_evt">마우스 이벤트 종류</param>
    private void OnMouseClicked(Define.MouseEvent _evt)
    {
        if (state == PlayerState.Die) return;
        if (_evt != Define.MouseEvent.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        
        RaycastHit hit;
        LayerMask layer = LayerMask.GetMask("Wall") | LayerMask.GetMask("Ground");
        if(Physics.Raycast(ray, out hit, 100f, layer))
        {
            destPos = hit.point;
            state = PlayerState.Moving;
        }
    }

    #region Event Callback
    private void OnRunEvent()
    {
        Debug.Log("터벅");
    }
    #endregion
}