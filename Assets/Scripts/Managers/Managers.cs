using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    private InputManager input = new InputManager();
    private ResourceManager resource = new ResourceManager();
    private UIManager ui = new UIManager();
    private SceneManagerEx scene = new SceneManagerEx();
    private SoundManager sound = new SoundManager();

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
    public static UIManager UI => Instance.ui;
    public static SceneManagerEx Scene => Instance.scene;
    public static SoundManager Sound => Instance.sound;

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

            s_instance.sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
    }

    private void Update()
    {
        Input.OnUpdate();
    }
}
