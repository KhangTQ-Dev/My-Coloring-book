using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingStateManager : unity_base.Singleton<LoadingStateManager>
{
    [SerializeField] Text txtInfor;
    [SerializeField] RectTransform rect;
    private void Start()
    {
        rect.anchoredPosition = new Vector2(0, -(Screen.height - Screen.safeArea.yMax) - 5);
    }

    public void SetText(string txt)
    {
        txtInfor.text = txt;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
