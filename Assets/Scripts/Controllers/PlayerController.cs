using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : CharacterController
{
    private PlayerStat stat;
    private FixedJoystick joystick;
    private int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);  // 레이어 마스크
    private bool stopAttack = false;
    
    public bool autoAttack = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        stat = GetComponent<PlayerStat>();
        joystick = Managers.UI.GetScene<UI_Joystick>().Joystick;

       // Managers.Input.MouseAction -= OnMouseEvent;
       // Managers.Input.MouseAction += OnMouseEvent;

        if (GetComponentInChildren<UI_Hpbar>() == null)
        {
            Managers.UI.MakeWordSpaceUI<UI_Hpbar>(transform);
        }
    }

    protected override void UpdateIdle()
    {
        if(joystick != null)
        {
            if(joystick.Horizontal != 0 && joystick.Vertical != 0)
            {
                State = Define.CharacterState.Moving;
                return;
            }
        }

        if(autoAttack)
        {
            ScanTarget();
        }
    }

    /// <summary>
    /// 플레이어 상태가 Moving일 경우 실행되는 업데이트 함수
    /// </summary>
    protected override void UpdateMoving()
    {
        // TODO: 타겟이 있을 경우, 몬스터가 사정거리에 들어오는 로직 실행
        if (target != null)
        {
            if(joystick != null)
            {
                if(joystick.Horizontal != 0 || joystick.Vertical != 0)
                {
                    NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                    nav.ResetPath();
                    target = null;
                    Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.white);
                    return;
                }
            }

            float distance = (target.transform.position - transform.position).magnitude;
            if (distance <= 2f)
            {
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.SetDestination(transform.position);
                State = Define.CharacterState.Attack;
                return;
            }
            else
            {   // TODO: 아직 공격범위에 들어가지 않아서 이동
                Vector3 dir = target.transform.position - transform.position;
                dir.y = 0;

                if (dir.magnitude < 0.1f) State = Define.CharacterState.Idle;
                else
                {
                    NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                    nav.SetDestination(target.transform.position);
                    nav.speed = stat.MoveSpeed;

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
                }
            }
        }
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            nav.ResetPath();
            Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.white);

            if (joystick != null)
            {
                if (joystick.Horizontal == 0 && joystick.Vertical == 0)
                {
                    if(autoAttack)
                    {
                        ScanTarget();
                    }
                    else
                    {
                        State = Define.CharacterState.Idle;
                    }
                    return;
                }
            }

            Debug.Log(State);
            Vector3 dir = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            dir.y = 0;
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        //// TODO: 타겟이 있을 경우, 몬스터가 사정거리에 들어오는 로직 실행
        //if(target != null)
        //{
        //    float distance = (destPos - transform.position).magnitude;
        //    if(distance <= 1)
        //    {
        //        State = Define.CharacterState.Attack;
        //        return;
        //    }
        //}

        //Vector3 dir = destPos - transform.position;     // 목표지점의 방향벡터
        //dir.y = 0;
        //if (dir.magnitude < 0.1f) State = Define.CharacterState.Idle;
        //else
        //{
        //    if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")))
        //    {
        //        if(!Input.GetMouseButton(0))
        //        {
        //            State = Define.CharacterState.Idle;
        //        }
        //        return;
        //    }

        //    float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
        //    transform.position += dir.normalized * moveDist;
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        //}
    }

    /// <summary>
    /// 플레이어 상태가 Attack일 경우 실행되는 업데이트 함수
    /// </summary>
    protected override void UpdateAttack()
    {
        if (joystick != null)
        {
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                State = Define.CharacterState.Moving;
                Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.white);
                return;
            }
        }

        if (target == null)
        {
            State = Define.CharacterState.Idle;

            if(autoAttack)
            {
                ScanTarget();
            }
            else
            {
                Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.white);
            }
            return;
        }

        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 마우스 입력 움직임 제어 함수
    /// </summary>
    /// <param name="_evt">마우스 이벤트 종류</param>
    private void OnMouseEvent(Define.MouseEvent _evt)
    {
        switch(State)
        {
            case Define.CharacterState.Idle: case Define.CharacterState.Moving: OnMouseEvent_IdleRun(_evt); break;
            case Define.CharacterState.Attack:
                {
                    if(_evt == Define.MouseEvent.PointerUp)
                    {   // TODO: 한번이라도 공격을 멈추면 중지
                        stopAttack = true;
                    }
                }
                break;
        }
    }

    private void OnMouseEvent_IdleRun(Define.MouseEvent _evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100f, mask);

        switch (_evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        destPos = hit.point;
                        State = Define.CharacterState.Moving;
                        stopAttack = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {   // TODO: 몬스터 클릭 이벤트
                            target = hit.collider.gameObject;
                        }
                        else
                        {   // TODO: 지형 클릭 이벤트
                            target = null;
                        }
                    }
                }
                break;

            case Define.MouseEvent.Press:
                {
                    if (target == null && raycastHit)
                    {
                        destPos = hit.point;
                    }
                }
                break;

            case Define.MouseEvent.PointerUp:
                {
                    stopAttack = true;
                }
                break;
        }
    }

    public void ScanTarget()
    {
        if(target != null)
        {
            target = null;
            Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.white);
            return;
        }
        else
        {
            GameObject closestObject = null;
            float closestDistance = Mathf.Infinity;

            Collider[] finds = Physics.OverlapSphere(transform.position, 50f, 1 << (int)Define.Layer.Monster);
            foreach(Collider find in finds)
            {
                GameObject obj = find.gameObject;
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if(distance < closestDistance)
                {
                    closestObject = obj;
                    closestDistance = distance;
                }
            }
            target = closestObject;
        }

        if (target != null)
        {
            State = Define.CharacterState.Moving;
            Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.yellow);
        }
    }

    public void ResetTarget()
    {
        if(target != null)
        {
            target = null;
            Managers.UI.GetScene<UI_AttackButton>().SetAttackButtonColor(Color.white);
        }
    }

    #region Event Callback
    private void OnRunEvent()
    {
        Debug.Log("터벅");
    }

    private void OnAttackEvent()
    {
        Debug.Log("공격");

        if(target != null)
        {
            Stat targetStat = target.GetComponent<Stat>();
            targetStat.OnAttacked(stat);
        }

        if (stopAttack) State = Define.CharacterState.Idle;
        else State = Define.CharacterState.Attack;
    }
    #endregion
}