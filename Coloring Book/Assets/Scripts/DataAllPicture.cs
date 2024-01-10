using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Tool/DataAllPicture")]
public class DataAllPicture : SerializedScriptableObject
{
    public List<List<DataPicture>> dataPictures;

    public List<DataPicture> dataPicturesAll;

    [Button]
    public void Setup()
    {
        for(int i = 0; i < dataPictures.Count; i++)
        {
            for(int j = 0; j < dataPictures[i].Count; j++)
            {
                dataPictures[i][j].TypeGallery = (TypeGallery)i;

                dataPictures[i][j].TypeId = (TypeId)j;
            }
        }
    }
}
