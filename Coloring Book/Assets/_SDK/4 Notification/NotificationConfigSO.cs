using System.Collections.Generic;
using UnityEngine;

public enum ELangguage
{
    EN, JA
}
[System.Serializable]
public class LanguageAndContent
{
    public ELangguage code;

}
[System.Serializable]
public class PushInfor
{
    public int Day;
    public int Hour;
    public int Minutes;
    public string Title = string.Empty;
    public string Description = string.Empty;
}
[CreateAssetMenu(fileName = "NotificationConfigSO", menuName = "Notification Config/Notification Config SO", order = 1)]

public class NotificationConfigSO : ScriptableObject
{
    public List<PushInfor> pushInfors;
}
