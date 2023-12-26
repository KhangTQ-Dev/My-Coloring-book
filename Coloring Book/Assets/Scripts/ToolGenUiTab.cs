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
        ButtonExtension[] objButton = FindObjectsOfType<ButtonExtension>();

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

    [Button]
    public void SetupToButtonExtension()
    {
        Button[] objButton = FindObjectsOfType<Button>();

        for (int i = 0; i < objButton.Length; i++)
        {
            GameObject buttonObj = objButton[i].gameObject;

            if (buttonObj.TryGetComponent<ButtonExtension>(out ButtonExtension a))
            {
                continue;
            }

            DestroyImmediate(objButton[i]);

            buttonObj.AddComponent<ButtonExtension>();
        }
    }
}