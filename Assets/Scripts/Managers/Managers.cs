using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    private InputManager input = new InputManager();
    private ResourceManager resource = new ResourceManager();

    public static Managers Instance
    {
        get
        {
            Init();
            return s_instance;   
        }
    }
    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if(s_instance == null)
        {
            GameObject target = GameObject.Find("@Managers");
            if(target == null)
            {
                target = new GameObject { name = "@Managers" };
                target.AddComponent<Managers>();
            }

            DontDestroyOnLoad(target);
            s_instance = target.GetComponent<Managers>();
        }
    }

    private void Update()
    {
        input.OnUpdate();
    }
}
