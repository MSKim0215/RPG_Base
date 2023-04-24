using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 destPos;            // 목표지점
    private bool isMoveToDest = false;  // 목표지점 이동 활성화 체크

    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;

        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private void Update()
    {
        if(isMoveToDest)
        {
            Vector3 dir = destPos - transform.position;     // 목표지점의 방향벡터
            if (dir.magnitude < 0.0001f) isMoveToDest = false;
            else
            {
                float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
                transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 키 입력 움직임 제어 함수
    /// </summary>
    private void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * speed;
        }

        isMoveToDest = false;
    }

    /// <summary>
    /// 마우스 입력 움직임 제어 함수
    /// </summary>
    /// <param name="_evt">마우스 이벤트 종류</param>
    private void OnMouseClicked(Define.MouseEvent _evt)
    {
        if (_evt != Define.MouseEvent.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Wall")))
        {
            destPos = hit.point;
            isMoveToDest = true;
        }
    }
}