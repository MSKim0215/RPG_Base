using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class CharacterController : MonoBehaviour
{
    protected Vector3 destPos;            // 목표지점
    protected GameObject target;

    [Header("플레이어 상태")]
    [SerializeField] protected Define.CharacterState state = Define.CharacterState.Idle;

    public virtual Define.CharacterState State
    {
        get { return state; }
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
            }
        }
    }

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (State)
        {
            case Define.CharacterState.Die: UpdateDie(); break;
            case Define.CharacterState.Idle: UpdateIdle(); break;
            case Define.CharacterState.Moving: UpdateMoving(); break;
            case Define.CharacterState.Attack: UpdateAttack(); break;
        }
    }

    public abstract void Init();

    protected virtual void UpdateDie() { }

    protected virtual void UpdateIdle() { }

    protected virtual void UpdateMoving() { }

    protected virtual void UpdateAttack() { }
}