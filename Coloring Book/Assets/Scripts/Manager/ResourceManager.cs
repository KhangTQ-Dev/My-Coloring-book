using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, ISingleton
{
    public static ResourceManager Instance;

    private Dictionary<string, GameObject> dicLoad;

    public void InitSingleton()
    {
        Instance = this;

        dicLoad = new Dictionary<string, GameObject>();
    }

    public GameObject Load(string path)
    {
        if (!dicLoad.ContainsKey(path))
        {
            GameObject objLoad = Resources.Load<GameObject>(path);

            dicLoad.Add(path, objLoad);

            return objLoad;
        }
        else
        {
            return dicLoad[path];
        }
    }

    private void OnDestroy()
    {
        //foreach(var item in dicLoad)
        //{
        //    if(item.Value != null)
        //    {
        //        Resources.UnloadAsset(item.Value);
        //    }
        //}

        dicLoad.Clear();
    }
}
