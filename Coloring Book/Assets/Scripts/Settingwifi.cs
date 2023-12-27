using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settingwifi : MonoBehaviour
{

 
    // Start is called before the first frame update
    public void SettingInternet()
    {
#if !UNITY_EDITOR
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivityObject =
            unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var intentObject = new AndroidJavaObject(
            "android.content.Intent", "android.settings.WIFI_SETTINGS"))
        {
            currentActivityObject.Call("startActivity", intentObject);
        }
#endif

        GameManager.Instance.CheckInternet();

    }
}
