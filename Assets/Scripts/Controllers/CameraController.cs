using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Define.CameraMode mode = Define.CameraMode.QuarterView;
    private GameObject player;
    private Vector3 delta;       // ī�޶�� �÷��̾��� �Ÿ�

    private void Start()
    {
        player = GameObject.Find("Player");
        delta = new Vector3(0, 6, -5);
    }

    private void LateUpdate()
    {
        if (mode == Define.CameraMode.QuarterView)
        {
            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
            {   // TODO: ī�޶�� �÷��̾� ���̿� ���� �ִٸ� ī�޶� �̵�
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * dist;
            }
            else
            {   // TODO: �ƹ��͵� ���ٸ� �Ϲ����� �̵�
                transform.position = player.transform.position + delta;
                transform.LookAt(player.transform);
            }
        }
    }

    public void SetQuarterView(Vector3 _delta)
    {
        mode = Define.CameraMode.QuarterView;
        delta = _delta;
    }
}