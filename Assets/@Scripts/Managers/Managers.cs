using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers instance;

    public static Managers Instance { get { Init(); return instance; } }


    //Core Managers

    TimeManager _time = new TimeManager();
    UIManager _ui = new UIManager();
    SaveManager _save = new SaveManager();
    ResourceManager _resource = new ResourceManager();
    GameManager _game = new GameManager();
    MySceneManager _scene = new MySceneManager();



    public static TimeManager Time { get { return Instance?._time; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static SaveManager Save { get { return Instance?._save; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static GameManager Game { get { return Instance?._game; } }
    public static MySceneManager Scene { get { return Instance?._scene; } }


    //Content Managers




    public static void Init()
    {
        if (instance != null)
            return;

        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);
        instance = go.GetComponent<Managers>();

        instance._save.Init();
        instance._time.Init();
        instance._ui.Init();
        instance._scene.Init();
    }

    public static void Clear()
    {
        Resource.Clear();
        UI.Clear();
    }



}
