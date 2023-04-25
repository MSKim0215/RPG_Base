using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string _path) where T : Object => Resources.Load<T>(_path);

    /// <summary>
    /// 프리팹 생성 함수
    /// </summary>
    /// <param name="_path">프리팹 경로 {Default: Prefabs/}</param>
    /// <param name="_parent">프리팹 부모</param>
    /// <returns></returns>
    public GameObject Instantiate(string _path, Transform _parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{_path}");
        if(prefab == null)
        {
            Debug.LogWarning($"프리팹을 찾을 수 없습니다. {_path}");
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