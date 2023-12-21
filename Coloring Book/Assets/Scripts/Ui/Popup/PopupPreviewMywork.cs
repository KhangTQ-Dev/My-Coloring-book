using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPreviewMywork : UiCanvas
{
    private TypeGallery typeGallery;

    private TypeId typeId;

    [SerializeField] private Button btnContinue;

    [SerializeField] private Button btnSave;

    [SerializeField] private Button btnHome;

    [SerializeField] private Button btnDelete;

    [SerializeField] private Button btnRenew;

    [SerializeField] private Transform parentInstance;

    [SerializeField] private PopupDelete popupDelete;

    [SerializeField] private PopupRenew popupRenew;

    [SerializeField] private PictureManager pictureManager;

    protected override void Start()
    {
        base.Start();

        btnContinue.onClick.AddListener(OnClickBtnContinue);

        btnSave.onClick.AddListener(OnClickBtnSave);

        btnHome.onClick.AddListener(OnClickBtnHome);

        btnDelete.onClick.AddListener(OnClickBtnDelete);

        btnRenew.onClick.AddListener(OnClickBtnRenew);
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

    private void OnClickBtnDelete()
    {
        popupDelete.Init(() => 
        {
            GameManager.Instance.DataManager.DeleteDataPicture(LevelManager.Instance.GamePlayManager.GetCurrentTypeGallery(), LevelManager.Instance.GamePlayManager.GetCurrentTypeId());

            LevelManager.Instance.OnChangeToLobby();

            Show(false);
        });

        popupDelete.Show(true);
    }

    private void OnClickBtnRenew()
    {
        popupRenew.Init(() => 
        {
            GameManager.Instance.DataManager.DeleteDataPicture(LevelManager.Instance.GamePlayManager.GetCurrentTypeGallery(), LevelManager.Instance.GamePlayManager.GetCurrentTypeId());

            LevelManager.Instance.GamePlayManager.RenewPicture();

            Show(false);
        });

        popupRenew.Show(true);
    }
}
