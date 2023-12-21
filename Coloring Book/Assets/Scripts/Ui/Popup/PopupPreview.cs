using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPreview : UiCanvas
{
    private TypeGallery typeGallery;

    private TypeId typeId;

    [SerializeField] private Button btnContinue;

    [SerializeField] private Button btnSave;

    [SerializeField] private Button btnHome;

    [SerializeField] private Transform parentInstance;

    [SerializeField] private PictureManager pictureManager;

    protected override void Start()
    {
        base.Start();

        btnContinue.onClick.AddListener(OnClickBtnContinue);

        btnSave.onClick.AddListener(OnClickBtnSave);

        btnHome.onClick.AddListener(OnClickBtnHome);
    }

    public override void Show(bool _isShow)
    {
        if (_isShow)
        {
            if (pictureManager != null)
            {
                Destroy(pictureManager);
            }

            TypeGallery typeGallery = LevelManager.Instance.GamePlayManager.GetCurrentTypeGallery();

            TypeId typeId = LevelManager.Instance.GamePlayManager.GetCurrentTypeId();

            DataPicture dataPicture = GameManager.Instance.DataManager.GetDataPicture(typeGallery, typeId);

            GameObject objInstance = Instantiate<GameObject>(dataPicture.prefabUiPicture, parentInstance);

            pictureManager = objInstance.GetComponent<PictureManager>();

            pictureManager.Init(typeGallery, typeId);
        }



        base.Show(_isShow);
    }

    private void OnClickBtnContinue()
    {
        Show(false);
    }

    private void OnClickBtnSave()
    {

    }

    private void OnClickBtnHome()
    {

        LevelManager.Instance.OnChangeToLobby();

        Show(false);

    }
}
