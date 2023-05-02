using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Define.CameraMode mode = Define.CameraMode.QuarterView;
    private Vector3 delta;       // 카메라와 플레이어의 거리
    private GameObject player;

    private void Start()
    {
        delta = new Vector3(0, 9, -7.5f);
    }

    private void LateUpdate()
    {
        if (mode == Define.CameraMode.QuarterView)
        {
            if (!player.IsValid()) return;

            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, 1 << (int)Define.Layer.Block))
            {   // TODO: 카메라와 플레이어 사이에 벽이 있다면 카메라 이동
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * dist;
            }
            else
            {   // TODO: 아무것도 없다면 일반적인 이동
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

    public void SetPlayer(GameObject _player) => player = _player;
}