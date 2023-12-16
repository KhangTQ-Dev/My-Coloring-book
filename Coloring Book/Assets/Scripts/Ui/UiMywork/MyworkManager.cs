using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyworkManager : MonoBehaviour
{
    [SerializeField] private Canvas canvasMywork;

    private bool isShow;

    void Start()
    {
        
    }

    public void Show(bool isTrue)
    {
        canvasMywork.enabled = isTrue;

        if (isTrue && !isShow)
        {
            isShow = isTrue;

            Init();
        }
    }

    public void Init()
    {

    }
}
