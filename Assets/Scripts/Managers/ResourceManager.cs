using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string _path) where T : Object => Resources.Load<T>(_path);

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

        GameObject result = Object.Instantiate(prefab, _parent);
        int index = result.name.IndexOf("(Clone)");
        if(index > 0) result.name = result.name.Substring(0, index);
        return result;
    }

    public void Destroy(GameObject _target)
    {
        if (_target == null) return;
        Object.Destroy(_target);
    }
}