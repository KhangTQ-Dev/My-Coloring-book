using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Tool/DataPicture")]
public class DataPicture : SerializedScriptableObject
{
    public GameObject prefabPicture;

    public GameObject prefabUiPicture;

    public TypeGallery TypeGallery;

    public TypeId TypeId; 
}