using AlmostEngine.Screenshot;
using AlmostEngine.Screenshot.Extra;
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

    [SerializeField] private CutScreenshotPostProcess cutScreenshotPostProcess;

    [SerializeField] private float indexScale;

    ScreenshotManager m_ScreenshotManager;

    protected override void Start()
    {
        base.Start();

        m_ScreenshotManager = GameObject.FindObjectOfType<ScreenshotManager>();

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
                Destroy(pictureManager.gameObject);
            }

            TypeGallery typeGallery = LevelManager.Instance.GamePlayManager.GetCurrentTypeGallery();

            TypeId typeId = LevelManager.Instance.GamePlayManager.GetCurrentTypeId();

            DataPicture dataPicture = GameManager.Instance.DataManager.GetDataPicture(typeGallery, typeId);

            GameObject objInstance = Instantiate<GameObject>(dataPicture.prefabUiPicture, parentInstance);

            objInstance.transform.localScale = objInstance.transform.localScale * indexScale;

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
        //parentInstance.transform.parent = canvasCapture;

        //LevelManager.Instance.gameObject.SetActive(false);

        cutScreenshotPostProcess.m_SelectionArea = parentInstance.GetComponent<RectTransform>();

        if (m_ScreenshotManager)
        {
            m_ScreenshotManager.Capture();
        }

        //StartCoroutine(WaitCapture());
    }

    IEnumerator WaitCapture()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        parentInstance.transform.parent = transform;

        LevelManager.Instance.gameObject.SetActive(true);
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
