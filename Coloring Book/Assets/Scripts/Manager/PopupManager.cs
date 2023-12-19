using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private List<UiCanvas> listPopups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopup(TypePopup typePopup, Action actionPopupDone = null)
    {
        listPopups[(int)typePopup].Show(true);
    }
}

public enum TypePopup
{
    LoadingToGamePlay
}