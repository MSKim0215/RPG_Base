using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject player;
    private HashSet<GameObject> monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() => player;

    /// <summary>
    /// 게임 오브젝트 스폰 함수
    /// </summary>
    /// <param name="_type">오브젝트 타입</param>
    /// <param name="_path">오브젝트 경로</param>
    /// <param name="_parent">생성될 위치</param>
    /// <returns></returns>
    public GameObject Spawn(Define.WorldObject _type, string _path, Transform _parent = null)
    {
        GameObject prefab = Managers.Resource.Instantiate(_path, _parent);
        switch(_type)
        {
            case Define.WorldObject.Monster:
                {
                    monsters.Add(prefab);
                    OnSpawnEvent?.Invoke(1);
                }
                break;
            case Define.WorldObject.Player: player = prefab; break;
        }
        return prefab;
    }

    /// <summary>
    /// 오브젝트의 타입을 리턴하는 함수
    /// </summary>
    /// <param name="_obj">검사할 오브젝트</param>
    /// <returns></returns>
    public Define.WorldObject GetWorldObjectType(GameObject _target)
    {
        CharacterControllerEx character = _target.GetComponent<CharacterControllerEx>();
        if (character == null) return Define.WorldObject.Unknown;
        return character.WorldObjectType;
    }

    /// <summary>
    /// 게임 오브젝트 제거 함수
    /// </summary>
    /// <param name="_target">오브젝트</param>
    public void Despawn(GameObject _target)
    {
        Define.WorldObject type = GetWorldObjectType(_target);
        switch(type)
        {
            case Define.WorldObject.Monster:
                {
                    if(monsters.Contains(_target))
                    {
                        monsters.Remove(_target);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;
            case Define.WorldObject.Player:
                {
                    if(player == _target)
                    {
                        player = null;
                    }
                }
                break;
        }
        Managers.Resource.Destroy(_target);
    }
}