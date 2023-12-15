using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DataManager : SerializedMonoBehaviour
{
    [SerializeField] private DataAllPicture dataAllPicture;

    public DataPicture GetDataPicture(TypeGallery typeGallery, TypeId  typeId)
    {
        return dataAllPicture.dataPictures[(int)typeGallery][(int)typeId];
    }

    public void SavePicture()
    {

    }

    public void LoadPicture()
    {

    }

    public void GetDataTypePicture()
    {

    }

    public bool GetMusic()
    {
        return PlayerPrefs.GetInt("Music", 1) == 1? true : false;
    }

    public void SetMusic(bool isTrue)
    {
        if (isTrue)
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
        }
    }

    public bool GetSound()
    {
        return PlayerPrefs.GetInt("Sound", 1) == 1 ? true : false;
    }

    public void SetSound(bool isTrue)
    {
        if (isTrue)
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
        }
    }
}
