 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    private UI_Scene scene;
    private int order = 10;      // ���� ĵ������ ����

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if(root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject _target, bool _sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(_target);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (_sort)
        {   // TODO: �˾� UI�� order ����
            canvas.sortingOrder = order;
            order++;
        }
        else
        {   // TODO: �Ϲ� UI�� order 0
            canvas.sortingOrder = 0;
        }
    }

    #region Scene ����
    /// <summary>
    /// �� UI�� ȣ���ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T">��ũ��Ʈ �̸�</typeparam>
    /// <param name="_name">������ �̸�</param>
    /// <returns></returns>
    public T ShowSceneUI<T>(string _name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Instantiate($"UI/Scene/{_name}");
        T sceneUI = Util.GetOrAddComponent<T>(prefab);

        prefab.transform.SetParent(Root.transform);

        return sceneUI;
    }
    #endregion

    #region Popup ����
    /// <summary>
    /// �˾� UI�� ȣ���ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T">��ũ��Ʈ �̸�</typeparam>
    /// <param name="_name">������ �̸�</param>
    /// <returns></returns>
    public T ShowPopupUI<T>(string _name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Instantiate($"UI/Popup/{_name}");
        T popup = Util.GetOrAddComponent<T>(prefab);
        popupStack.Push(popup);

        prefab.transform.SetParent(Root.transform);

        return popup;
    }

    /// <summary>
    /// �ݴ� �˾� UI�� �´��� �� �� �ݴ� �Լ�
    /// </summary>
    /// <param name="_popup">������ �˾�</param>
    public void ClosePopupUI(UI_Popup _popup)
    {
        if (popupStack.Count == 0) return;

        if(popupStack.Peek() != _popup)
        {
            Debug.LogWarning("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }

    /// <summary>
    /// �˾� UI�� �ݴ� �Լ�
    /// </summary>
    public void ClosePopupUI()
    {
        if (popupStack.Count == 0) return;

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);

        order--;
    }

    /// <summary>
    /// ��� �˾� UI�� �ݴ� �Լ�
    /// </summary>
    public void CloseAllPopupUI()
    {
        while(popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
    #endregion
}