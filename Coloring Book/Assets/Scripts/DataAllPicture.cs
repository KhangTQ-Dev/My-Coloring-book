using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Tool/DataAllPicture")]
public class DataAllPicture : SerializedScriptableObject
{
    public List<List<DataPicture>> dataPictures;
}
