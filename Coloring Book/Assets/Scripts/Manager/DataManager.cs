using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DataManager : SerializedMonoBehaviour
{
    [SerializeField] private DataAllPicture dataAllPicture;

    [SerializeField] private Color colorDefaul;

    public bool CheckIsSave(TypeGallery typeGallery, TypeId typeId)
    {
        string path = "DataPicture" + typeGallery.ToString() + typeId.ToString();

        if (PlayerPrefs.HasKey(path))
        {
            string a = PlayerPrefs.GetString(path, "");

            DataColorPicture b = JsonUtility.FromJson<DataColorPicture>(a);

            return b.IsSave;
        }
        else
        {
            return false;
        }
    }

    public List<DataPicture> GetSaveDataPicture()
    {
        return new List<DataPicture>();
    }

    public DataPicture GetDataPicture(TypeGallery typeGallery, TypeId  typeId)
    {
        return dataAllPicture.dataPictures[(int)typeGallery][(int)typeId];
    }

    public void SavePicture(TypeGallery typeGallery, TypeId typeId, DataColorPicture dataColorPicture)
    {
        string a = JsonUtility.ToJson(dataColorPicture);

        PlayerPrefs.SetString("DataPicture" + typeGallery.ToString() + typeId.ToString(), a);
    }

    public DataColorPicture LoadPicture(TypeGallery typeGallery, TypeId typeId, int numberPiece)
    {
        string path = "DataPicture" + typeGallery.ToString() + typeId.ToString();

        if (PlayerPrefs.HasKey(path))
        {
            string a = PlayerPrefs.GetString(path, "");

            DataColorPicture b = JsonUtility.FromJson<DataColorPicture>(a);

            return b;
        }
        else 
        {
            DataColorPicture dataColorPicture = new DataColorPicture();

            dataColorPicture.DataElementColorPictures = new List<Color>();

            dataColorPicture.IsSave = false;

            //DataElementColorPicture dataElementColorPicture = new DataElementColorPicture() { Color = colorDefaul };

            for(int i = 0; i < numberPiece; i++)
            {
                dataColorPicture.DataElementColorPictures.Add(colorDefaul);
            }

            SavePicture(typeGallery, typeId, dataColorPicture);

            return dataColorPicture;
        }
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

public class DataColorPicture
{
    public List<Color> DataElementColorPictures;

    public bool IsSave;
}

public class DataElementColorPicture
{
    public Color Color;
}