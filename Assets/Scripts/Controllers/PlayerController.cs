using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : CharacterController
{
    private PlayerStat stat;
    private FixedJoystick joystick;
    private int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);  // ���̾� ����ũ
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
    /// �÷��̾� ���°� Moving�� ��� ����Ǵ� ������Ʈ �Լ�
    /// </summary>
    protected override void UpdateMoving()
    {
        // TODO: Ÿ���� ���� ���, ���Ͱ� �����Ÿ��� ������ ���� ����
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
            {   // TODO: ���� ���ݹ����� ���� �ʾƼ� �̵�
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

        //// TODO: Ÿ���� ���� ���, ���Ͱ� �����Ÿ��� ������ ���� ����
        //if(target != null)
        //{
        //    float distance = (destPos - transform.position).magnitude;
        //    if(distance <= 1)
        //    {
        //        State = Define.CharacterState.Attack;
        //        return;
        //    }
        //}

        //Vector3 dir = destPos - transform.position;     // ��ǥ������ ���⺤��
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
    /// �÷��̾� ���°� Attack�� ��� ����Ǵ� ������Ʈ �Լ�
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
    /// ���콺 �Է� ������ ���� �Լ�
    /// </summary>
    /// <param name="_evt">���콺 �̺�Ʈ ����</param>
    private void OnMouseEvent(Define.MouseEvent _evt)
    {
        switch(State)
        {
            case Define.CharacterState.Idle: case Define.CharacterState.Moving: OnMouseEvent_IdleRun(_evt); break;
            case Define.CharacterState.Attack:
                {
                    if(_evt == Define.MouseEvent.PointerUp)
                    {   // TODO: �ѹ��̶� ������ ���߸� ����
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
                        {   // TODO: ���� Ŭ�� �̺�Ʈ
                            target = hit.collider.gameObject;
                        }
                        else
                        {   // TODO: ���� Ŭ�� �̺�Ʈ
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
        Debug.Log("�͹�");
    }

    private void OnAttackEvent()
    {
        Debug.Log("����");

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