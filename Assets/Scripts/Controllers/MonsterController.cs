using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : CharacterController
{
    private Stat stat;
    private float scanRange = 10f;      // 인식 범위
    private float attackRange = 2f;     // 공격 사거리

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        stat = GetComponent<Stat>();

        if(GetComponentInChildren<UI_Hpbar>() == null)
        {
            Managers.UI.MakeWordSpaceUI<UI_Hpbar>(transform);
        }
    }

    protected override void UpdateIdle()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = (player.transform.position - transform.position).magnitude;
        if(distance <= scanRange)
        {
            target = player;
            State = Define.CharacterState.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        // TODO: 타겟이 있을 경우, 플레이어가 사정거리에 들어오는 로직 실행
        if (target != null)
        {
            destPos = target.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if (distance <= attackRange)
            {
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.SetDestination(transform.position);
                State = Define.CharacterState.Attack;
                return;
            }
        }

        // TODO: 아직 공격범위에 들어가지 않아서 이동
        Vector3 dir = destPos - transform.position;     // 목표지점의 방향벡터
        if (dir.magnitude < 0.1f) State = Define.CharacterState.Idle;
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            nav.SetDestination(destPos);
            nav.speed = stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateDie()
    {
        Debug.Log("몬스터 죽음");
    }

    protected override void UpdateAttack()
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20f * Time.deltaTime);
        }
    }

    #region Event Callback
    private void OnRunEvent()
    {
        Debug.Log("몬스터벅");
    }

    private void OnAttackEvent()
    {
        if (target != null)
        {
            Stat targetStat = target.GetComponent<Stat>();
            int damage = Mathf.Max(0, stat.Attack - targetStat.Defense);
            targetStat.Hp -= damage;

            if(targetStat.Hp <= 0)
            {
                Managers.Game.Despawn(targetStat.gameObject);
            }

            if (targetStat.Hp > 0)
            {
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance <= attackRange) State = Define.CharacterState.Attack;
                else State = Define.CharacterState.Moving;
            }
            else State = Define.CharacterState.Idle;
        }
        else State = Define.CharacterState.Idle;
    }
    #endregion
}