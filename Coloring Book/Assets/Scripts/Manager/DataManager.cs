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
        List<DataPicture> dataPictures = new List<DataPicture>();

        for(int i = 0; i < dataAllPicture.dataPictures.Count; i++)
        {
            for(int j = 0; j < dataAllPicture.dataPictures[i].Count; j++)
            {
                TypeGallery typeGallery = (TypeGallery)i;

                TypeId typeId = (TypeId)j;

                if (LoadPicture(typeGallery, typeId, 0).IsSave)
                {
                    dataPictures.Add(dataAllPicture.dataPictures[i][j]);
                }
            }
        }

        return dataPictures;
    }

    public void DeleteDataPicture(TypeGallery typeGallery, TypeId typeId)
    {
        DataColorPicture dataColorPicture = LoadPicture(typeGallery, typeId);

        for(int i = 0; i < dataColorPicture.DataElementColorPictures.Count; i++)
        {
            dataColorPicture.DataElementColorPictures[i] = colorDefaul;
        }

        dataColorPicture.IsSave = false;

        SavePicture(typeGallery, typeId, dataColorPicture);
    }

    public DataPicture GetDataPicture(TypeGallery typeGallery, TypeId  typeId)
    {
        return dataAllPicture.dataPictures[(int)typeGallery][(int)typeId];
    }

    public void SetAdsDataPicture(TypeGallery typeGallery, TypeId typeId)
    {
        PlayerPrefs.SetInt("AdsPicture" + typeGallery.ToString() + typeId.ToString(), 1);
    }

    public bool GetAdsDataPicture(TypeGallery typeGallery, TypeId typeId)
    {
        if(PlayerPrefs.HasKey("AdsPicture" + typeGallery.ToString() + typeId.ToString()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SavePicture(TypeGallery typeGallery, TypeId typeId, DataColorPicture dataColorPicture)
    {
        string a = JsonUtility.ToJson(dataColorPicture);

        PlayerPrefs.SetString("DataPicture" + typeGallery.ToString() + typeId.ToString(), a);
    }

    public DataColorPicture LoadPicture(TypeGallery typeGallery, TypeId typeId)
    {
        string path = "DataPicture" + typeGallery.ToString() + typeId.ToString();

        string a = PlayerPrefs.GetString(path, "");

        DataColorPicture b = JsonUtility.FromJson<DataColorPicture>(a);

        return b;
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

        GameManager.Instance.SoundManager.SetVolumnBG(isTrue);
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

    public void SetShowRate()
    {
        PlayerPrefs.SetInt("ShowRate", 1);
    }

    public bool GetShowRate()
    {
        int a = PlayerPrefs.GetInt("ShowRate", 0);

        return a == 1 ? true : false;
    }

    public void SetFirstDone()
    {
        PlayerPrefs.SetInt("FirstDone", 1);
    }

    public bool GetFirstDone()
    {
        int a = PlayerPrefs.GetInt("FirstDone", 0);

        return a == 1 ? true : false;
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