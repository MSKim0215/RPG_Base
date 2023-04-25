using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject _target) where T : UnityEngine.Component
    {
        T component = _target.GetComponent<T>();
        if (component == null)
        {
            component = _target.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// �ڽ� ������Ʈ (���� ������Ʈ Ÿ�Կ�) Ž�� �Լ�
    /// </summary>
    /// <param name="_target">��ǥ ������Ʈ</param>
    /// <param name="_name">��ǥ ������Ʈ �̸� -> �̸��� ���� ������ �̸� �񱳾���</param>
    /// <param name="_recursive">�ڽĸ� ã�� ������ or ������ �ڽı��� ã�� ������</param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject _target, string _name = null, bool _recursive = false)
    {
        Transform transform = FindChild<Transform>(_target, _name, _recursive);
        if (transform != null) return transform.gameObject;
        return null;
    }

    /// <summary>
    /// �ڽ� ������Ʈ Ž�� �Լ�
    /// </summary>
    /// <typeparam name="T">��ǥ ������Ʈ Ÿ��</typeparam>
    /// <param name="_target">��ǥ ������Ʈ</param>
    /// <param name="_name">��ǥ ������Ʈ �̸� -> �̸��� ���� ������ �̸� �񱳾���</param>
    /// <param name="_recursive">�ڽĸ� ã�� ������ or ������ �ڽı��� ã�� ������</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject _target, string _name = null, bool _recursive = false) where T: UnityEngine.Object
    {
        if (_target == null) return null;

        if(!_recursive)
        {   // TODO: �ڽ��� ���� �ڽĸ� ã�� ���
            for(int i = 0; i < _target.transform.childCount; i++)
            {
                Transform transform = _target.transform.GetChild(i);
                if(string.IsNullOrEmpty(_name) || transform.name == _name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {   // TODO: �ڽ��� ������ �ڽı��� ã�� ���
            foreach(T component in _target.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(_name) || component.name == _name)
                {   
                    return component;
                }
            }
        }
        return null;
    }
}