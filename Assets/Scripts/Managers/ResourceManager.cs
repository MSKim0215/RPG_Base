using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string _path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {   // TODO: �̹� ���� ������Ʈ�� ��� �ִٸ� �ٷ� ���
            string name = _path;
            int index = name.LastIndexOf('\\');
            if(index >= 0) name = name.Substring(index + 1);

            GameObject original = Managers.Pool.GetOriginal(name);
            if (original != null) return original as T;
        }

        return Resources.Load<T>(_path);
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="_path">������ ��� {Default: Prefabs/}</param>
    /// <param name="_parent">������ �θ�</param>
    /// <returns></returns>
    public GameObject Instantiate(string _path, Transform _parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{_path}");
        if(prefab == null)
        {
            Debug.LogWarning($"�������� ã�� �� �����ϴ�. {_path}");
            return null;
        }

        if(prefab.GetComponent<Poolable>() != null)
        {   // TODO: Ǯ�� �ȿ� �ִ� ������Ʈ���� üũ
            return Managers.Pool.Pop(prefab, _parent).gameObject;
        }

        GameObject result = Object.Instantiate(prefab, _parent);
        result.name = prefab.name;
        return result;
    }

    public void Destroy(GameObject _target)
    {
        if (_target == null) return;

        Poolable able = _target.GetComponent<Poolable>();
        if(able != null)
        {   // TODO: Ǯ������ ���Ǵ� ������Ʈ���� üũ
            Managers.Pool.Push(able);
            return;
        }
        Object.Destroy(_target);
    }
}