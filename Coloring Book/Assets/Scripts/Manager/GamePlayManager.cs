using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Detect detect;

    public Detect Detect => detect;

    private TypeGallery typeGallery;

    private TypeId typeId;

    private PictureManager pictureManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(TypeGallery _typeGallery, TypeId _typeId)
    {
        typeGallery = _typeGallery;

        typeId = _typeId;

        LoadPicture(_typeGallery, _typeId);
    }

    public void Show(bool isTrue)
    {
        
    }

    public void LoadPicture(TypeGallery _typeGallery, TypeId _typeId)
    {
        DataPicture dataPicture = GameManager.Instance.DataManager.GetDataPicture(_typeGallery, _typeId);

        GameObject objLoad = Instantiate(dataPicture.prefabPicture, transform);

        pictureManager = objLoad.GetComponent<PictureManager>();

        pictureManager.Init(_typeGallery, _typeId);
    }

    public TypeGallery GetCurrentTypeGallery()
    {
        return typeGallery;
    }

    public TypeId GetCurrentTypeId()
    {
        return typeId;
    }
}