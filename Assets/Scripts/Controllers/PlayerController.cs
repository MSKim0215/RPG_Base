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

    private PlayerStat stat;
    private Vector3 destPos;            // 목표지점
    private int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);  // 레이어 마스크
    private GameObject target;

    [Header("플레이어 상태")]
    [SerializeField] private PlayerState state = PlayerState.Idle;

    public PlayerState State
    {
        get { return state; }
        set
        {
            Animator anim = GetComponent<Animator>();

            state = value;
            switch(state)
            {
                case PlayerState.Die: break;
                case PlayerState.Idle: anim.CrossFade("WAIT", 0.1f); break;
                case PlayerState.Moving: anim.CrossFade("RUN", 0.1f); break;
                case PlayerState.Attack: anim.CrossFade("ATTACK", 0.1f, -1, 0); break;
            }
        }
    }

    private void Start()
    {
        stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWordSpaceUI<UI_Hpbar>(transform);
    }

    private void Update()
    {
        switch (State)
        {
            case PlayerState.Die: UpdateDie(); break;
            case PlayerState.Idle: UpdateIdle(); break;
            case PlayerState.Moving: UpdateMoving(); break;
            case PlayerState.Attack: UpdateAttack(); break;
        }
    }

    private void UpdateDie()
    {

    }

    private void UpdateIdle()
    {
        // TODO: 애니메이션
    }

    /// <summary>
    /// 플레이어 상태가 Moving일 경우 실행되는 업데이트 함수
    /// </summary>
    private void UpdateMoving()
    {
        // TODO: 타겟이 있을 경우, 몬스터가 사정거리에 들어오는 로직 실행
        if(target != null)
        {
            float distance = (destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = PlayerState.Attack;
                return;
            }
        }

        Vector3 dir = destPos - transform.position;     // 목표지점의 방향벡터
        if (dir.magnitude < 0.1f) State = PlayerState.Idle;
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nav.Move(dir.normalized * moveDist);

            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")))
            {
                if(!Input.GetMouseButton(0))
                {
                    State = PlayerState.Idle;
                }
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        // TODO: 애니메이션
    }

    /// <summary>
    /// 플레이어 상태가 Attack일 경우 실행되는 업데이트 함수
    /// </summary>
    private void UpdateAttack()
    {
        if(target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20f * Time.deltaTime);
        }
    }

    private bool stopAttack = false;

    /// <summary>
    /// 마우스 입력 움직임 제어 함수
    /// </summary>
    /// <param name="_evt">마우스 이벤트 종류</param>
    private void OnMouseEvent(Define.MouseEvent _evt)
    {
        switch(State)
        {
            case PlayerState.Idle: case PlayerState.Moving: OnMouseEvent_IdleRun(_evt); break;
            case PlayerState.Attack:
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
                        State = PlayerState.Moving;
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
            PlayerStat myStat = GetComponent<PlayerStat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log(damage);
            targetStat.Hp -= damage;
        }

        if (stopAttack) State = PlayerState.Idle;
        else State = PlayerState.Attack;
    }
    #endregion
}