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
    private Vector3 destPos;            // ��ǥ����
    private PlayerState state = PlayerState.Idle;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();

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
        // TODO: �ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    /// <summary>
    /// �÷��̾� ���°� Moving�� ��� ����Ǵ� ������Ʈ �Լ�
    /// </summary>
    private void UpdateMoving()
    {
        Vector3 dir = destPos - transform.position;     // ��ǥ������ ���⺤��
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

        // TODO: �ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", stat.MoveSpeed);
    }

    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);  // ���̾� ����ũ
    /// <summary>
    /// ���콺 �Է� ������ ���� �Լ�
    /// </summary>
    /// <param name="_evt">���콺 �̺�Ʈ ����</param>
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
            {   // TODO: ���� Ŭ�� �̺�Ʈ
                Debug.Log("���� Ŭ��");
            }
            else
            {   // TODO: ���� Ŭ�� �̺�Ʈ
                Debug.Log("���� Ŭ��");
            }
        }
    }

    #region Event Callback
    private void OnRunEvent()
    {
        Debug.Log("�͹�");
    }
    #endregion
}