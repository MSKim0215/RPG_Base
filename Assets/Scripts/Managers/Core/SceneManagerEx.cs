using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    /// <summary>
    /// ���� Ÿ�Կ� ���� �ش� ���� �ε��ϴ� �Լ�
    /// </summary>
    /// <param name="_type">�� Ÿ��</param>
    public void LoadScene(Define.Scene _type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(_type));
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    private string GetSceneName(Define.Scene _type) => System.Enum.GetName(typeof(Define.Scene), _type);
}