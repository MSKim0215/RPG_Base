using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die, Idle, Moving
    }

    private float speed = 10f;
    private Vector3 destPos;            // ��ǥ����
    private PlayerState state = PlayerState.Idle;

    private void Start()
    {
        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;

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
        if (dir.magnitude < 0.0001f) state = PlayerState.Idle;
        else
        {
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        // TODO: �ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", speed);
    }

    ///// <summary>
    ///// Ű �Է� ������ ���� �Լ�
    ///// </summary>
    //private void OnKeyboard()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //        transform.position += Vector3.forward * Time.deltaTime * speed;
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //        transform.position += Vector3.back * Time.deltaTime * speed;
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //        transform.position += Vector3.right * Time.deltaTime * speed;
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //        transform.position += Vector3.left * Time.deltaTime * speed;
    //    }

    //    isMoveToDest = false;
    //}

    /// <summary>
    /// ���콺 �Է� ������ ���� �Լ�
    /// </summary>
    /// <param name="_evt">���콺 �̺�Ʈ ����</param>
    private void OnMouseClicked(Define.MouseEvent _evt)
    {
        if (state == PlayerState.Die) return;
        if (_evt != Define.MouseEvent.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Wall")))
        {
            destPos = hit.point;
            state = PlayerState.Moving;
        }
    }

    #region Event Callback
    private void OnRunEvent()
    {
        Debug.Log("�͹�");
    }
    #endregion
}