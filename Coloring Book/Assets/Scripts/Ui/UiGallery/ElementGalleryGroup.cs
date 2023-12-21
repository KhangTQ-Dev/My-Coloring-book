using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementGalleryGroup : MonoBehaviour
{
    [SerializeField] private DataPicture dataPicture;

    [SerializeField] private Button btnChoose;

    [SerializeField] private TypeGallery typeGallery;

    [SerializeField] private TypeId typeId;

    [SerializeField] private PictureManager pictureManager;

    [SerializeField] private Transform parentInstance;

    // Start is called before the first frame update
    void Start()
    {
        btnChoose.onClick.AddListener(OnClickBtnChoose);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickBtnChoose()
    {
        LevelManager.Instance.OnChangeToGamePlay(typeGallery, typeId);
    }

    public void Init()
    {
        pictureManager.Init(typeGallery, typeId);
    }

    public void InitAll()
    {
        typeGallery = dataPicture.TypeGallery;

        typeId = dataPicture.TypeId;
    }

    public void Setup(DataPicture _dataPicture)
    {
        dataPicture = _dataPicture;

        typeGallery = _dataPicture.TypeGallery;

        typeId = _dataPicture.TypeId;

        GameObject objInstance = Instantiate<GameObject>(dataPicture.prefabUiPicture, parentInstance);

        pictureManager = objInstance.GetComponent<PictureManager>();

        //pictureManager.Init(typeGallery, typeId);
    }
}