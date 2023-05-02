using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject player;
    private HashSet<GameObject> monsters = new HashSet<GameObject>();

    public GameObject GetPlayer() => player;

    /// <summary>
    /// ���� ������Ʈ ���� �Լ�
    /// </summary>
    /// <param name="_type">������Ʈ Ÿ��</param>
    /// <param name="_path">������Ʈ ���</param>
    /// <param name="_parent">������ ��ġ</param>
    /// <returns></returns>
    public GameObject Spawn(Define.WorldObject _type, string _path, Transform _parent = null)
    {
        GameObject prefab = Managers.Resource.Instantiate(_path, _parent);
        switch(_type)
        {
            case Define.WorldObject.Monster: monsters.Add(prefab); break;
            case Define.WorldObject.Player: player = prefab; break;
        }
        return prefab;
    }

    /// <summary>
    /// ������Ʈ�� Ÿ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="_obj">�˻��� ������Ʈ</param>
    /// <returns></returns>
    public Define.WorldObject GetWorldObjectType(GameObject _target)
    {
        CharacterController character = _target.GetComponent<CharacterController>();
        if (character == null) return Define.WorldObject.Unknown;
        return character.WorldObjectType;
    }

    /// <summary>
    /// ���� ������Ʈ ���� �Լ�
    /// </summary>
    /// <param name="_target">������Ʈ</param>
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