using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Singleton

    private static Manager _instance;
    private static bool initialized;
    public static Manager instance
    {
        get
        {
            if (!initialized)
            {
                initialized = true;

                GameObject obj = GameObject.Find("@Manager");
                if (obj == null)
                {
                    obj = new() { name = "@Manager" };
                    obj.AddComponent<Manager>();
                    DontDestroyOnLoad(obj);
                    _instance = obj.GetComponent<Manager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    #region Manage
    private readonly AssetManager asset = new();
    private readonly DataManager data = new();
    private readonly GameManager game = new();
    private readonly UiManager ui = new();
    private readonly ObjectPoolManager objectPool = new();

    public static AssetManager Asset => instance != null ? instance.asset : null;
    public static DataManager Data => instance != null ? instance.data : null;
    public static GameManager Game => instance != null ? instance.game : null;
    public static UiManager UI => instance != null ? instance.ui : null;
    public static ObjectPoolManager ObjectPool => instance != null ? instance.objectPool : null;
    #endregion
}
