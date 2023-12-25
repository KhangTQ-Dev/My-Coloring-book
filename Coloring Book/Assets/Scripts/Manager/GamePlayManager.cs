using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Detect detect;

    [SerializeField] private ZoomController zoomController;

    [SerializeField] private DrawFillController drawFillController;

    public ZoomController ZoomController => zoomController;

    public Detect Detect => detect;

    public DrawFillController DrawFillController => drawFillController;

    private TypeGallery typeGallery;

    private TypeId typeId;

    private PictureManager pictureManager;

    public PictureManager PictureManager => pictureManager;

    private bool canInteract;

    public bool CanInteract => canInteract;

    private List<TypeHistoryDraw> typeHistoryDraws;


    private int currentIdHistory;

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

        canInteract = true;

        detect.Init();

        zoomController.Init();

        typeHistoryDraws = new List<TypeHistoryDraw>();
    }

    public void Show(bool isTrue)
    {
        if (pictureManager != null && !isTrue)
        {
            Destroy(pictureManager.gameObject);
        }

        canInteract = isTrue;
    }

    public void LoadPicture(TypeGallery _typeGallery, TypeId _typeId)
    {
        if(pictureManager != null)
        {
            Destroy(pictureManager.gameObject);
        }

        DataPicture dataPicture = GameManager.Instance.DataManager.GetDataPicture(_typeGallery, _typeId);

        GameObject objLoad = Instantiate(dataPicture.prefabPicture, transform);

        pictureManager = objLoad.GetComponent<PictureManager>();

        pictureManager.Init(_typeGallery, _typeId);
    }

    public void RenewPicture()
    {
        pictureManager.Init(typeGallery, typeId);
    }

    public TypeGallery GetCurrentTypeGallery()
    {
        return typeGallery;
    }

    public TypeId GetCurrentTypeId()
    {
        return typeId;
    }

    public void AddHistory(int id, Color colorCurrent, Color colorChange)
    {
        typeHistoryDraws.Add(new TypeHistoryDraw() 
        {
            Id = id,
            CurrentColor = colorCurrent,
            ChangeColor = colorChange
        });

        currentIdHistory = typeHistoryDraws.Count;
    }

    public void BackHistory()
    {
        currentIdHistory--;

        TypeHistoryDraw typeHistoryDraw = typeHistoryDraws[currentIdHistory];

        pictureManager.ElementPiecePictures[typeHistoryDraw.Id].SetColor(typeHistoryDraw.CurrentColor);
    }

    public void NextHistory()
    {
        try
        {
            currentIdHistory++;

            TypeHistoryDraw typeHistoryDraw = typeHistoryDraws[currentIdHistory - 1];

            pictureManager.ElementPiecePictures[typeHistoryDraw.Id].SetColor(typeHistoryDraw.ChangeColor);
        }
        catch
        {

            //return typeHistoryDraws[typeHistoryDraws.Count];
        }
    }

    public bool CheckCanBackHistory()
    {
        if(currentIdHistory > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckCanNextHistory() 
    {
        if (currentIdHistory < typeHistoryDraws.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class TypeHistoryDraw
{
    public int Id;

    public Color CurrentColor;

    public Color ChangeColor;
}