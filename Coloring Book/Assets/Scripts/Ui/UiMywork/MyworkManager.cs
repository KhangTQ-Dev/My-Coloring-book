using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyworkManager : MonoBehaviour
{
    [SerializeField] private MyworkGroupManager myworkGroupManager;

    [SerializeField] private Canvas canvas;

    [SerializeField] private GraphicRaycaster graphicRaycaster;

    private bool isShow;

    void Start()
    {
        
    }

    public void Show(bool isTrue)
    {
        canvas.enabled = isTrue;

        graphicRaycaster.enabled = isTrue;

        if (isTrue)
        {
            Init();
        }

        isShow = isTrue;
    }

    public void Init()
    {
        myworkGroupManager.Init();
    }
}
