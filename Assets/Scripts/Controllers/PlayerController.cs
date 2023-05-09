using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : CharacterControllerEx
{
    private PlayerStat stat;
    private FixedJoystick joystick;

    public bool autoAttack = false;

    public override Define.CharacterState State
    {
        get => state;
        set
        {
            Animator anim = GetComponent<Animator>();

            state = value;
            switch (state)
            {
                case Define.CharacterState.Die: break;
                case Define.CharacterState.Idle: anim.CrossFade("WAIT", 0.1f); break;
                case Define.CharacterState.Moving: anim.CrossFade("RUN", 0.1f); break;
                case Define.CharacterState.Attack: anim.CrossFade("ATTACK", 0.1f, -1, 0); break;
                case Define.CharacterState.SkillAttack: anim.CrossFade("ATTACK", 0.1f, -1, 0); break;
                case Define.CharacterState.SkillBuff: Debug.Log("버프!"); break;
            }
        }
    }

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        stat = GetComponent<PlayerStat>();
        joystick = Managers.UI.GetScene<UI_Joystick>().Joystick;

        if (GetComponentInChildren<UI_Hpbar>() == null)
        {
            Managers.UI.MakeWordSpaceUI<UI_Hpbar>(transform);
        }
    }

    protected override void Update()
    {
        base.Update();

        switch(State)
        {
            case Define.CharacterState.SkillAttack: UpdateSkillAttack(); break;
            case Define.CharacterState.SkillBuff: UpdateSkillBuff(); break;
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

            Vector3 dir = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            dir.y = 0;
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
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

    private void UpdateSkillAttack()
    {

    }

    private void UpdateSkillBuff()
    {

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

        State = Define.CharacterState.Attack;
    }
    #endregion
}