using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ToolGenUiTab : SerializedMonoBehaviour
{
    [SerializeField] private DataAllPicture dataAllPicture;

    [SerializeField] private GalleryManager galleryManager;
 
    [Button]
    public void GenAll()
    {
        galleryManager.Setup(dataAllPicture);
    }
}