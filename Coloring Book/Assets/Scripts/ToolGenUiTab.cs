using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ToolGenUiTab : SerializedMonoBehaviour
{
    [SerializeField] private DataAllPicture dataAllPicture;

    [SerializeField] private GalleryManager galleryManager;
 
    [Button]
    public void GenAll()
    {
        galleryManager.Setup(dataAllPicture);
    }

    [Button]
    public void GenGroup()
    {
        galleryManager.SetupGroup(dataAllPicture);
    }

    [Button]
    public void GenTab()
    {
        galleryManager.SetupTab(dataAllPicture);
    }

    [Button]
    public void Init()
    {
        galleryManager.InitAll();
    }

    [Button]
    public void SetColorButton(Color color)
    {
        Button[] objButton = FindObjectsOfType<Button>();

        for (int i = 0; i < objButton.Length; i++)
        {
            ColorBlock colorBlock = objButton[i].colors;

            colorBlock.disabledColor = color;

            colorBlock.normalColor = color;

            colorBlock.selectedColor = color;

            colorBlock.pressedColor = color;

            objButton[i].colors = colorBlock;
        }
    }
}