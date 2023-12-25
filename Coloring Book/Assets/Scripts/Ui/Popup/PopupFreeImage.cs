using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupFreeImage : UiCanvas
{
    [SerializeField] private Button btnClose;

    [SerializeField] private Button btnClaimIt;

    [SerializeField] private Button btnNothanks;

    [SerializeField] private Transform parentInstance;

    [SerializeField] private PictureManager pictureManager;

    [SerializeField] private TypeGallery typeGallery;

    [SerializeField] private TypeId typeId;

    [SerializeField] private float indexScale;

    protected override void Start()
    {
        base.Start();

        btnClose.onClick.AddListener(OnClickBtnClose);

        btnClaimIt.onClick.AddListener(OnClickBtnClaimIt);

        btnNothanks.onClick.AddListener(OnClickBtnNothanks);
    }

    public override void Show(bool _isShow)
    {
        if (_isShow)
        {
            Init();
        }

        base.Show(_isShow);
    }

    public override void Init()
    {
        base.Init();
    }

    public void SetSpecialImage(TypeGallery _typeGallery, TypeId _typeId)
    {
        typeGallery = _typeGallery;

        typeId = _typeId;

        if (pictureManager != null)
        {
            Destroy(pictureManager.gameObject);
        }

        DataPicture dataPicture = GameManager.Instance.DataManager.GetDataPicture(typeGallery, typeId);

        GameObject objInstance = Instantiate<GameObject>(dataPicture.prefabUiPicture, parentInstance);

        objInstance.transform.localScale = objInstance.transform.localScale * indexScale;

        pictureManager = objInstance.GetComponent<PictureManager>();

        pictureManager.Init(typeGallery, typeId);
    }

    private void OnClickBtnClose()
    {
        Show(false);
    }

    private void OnClickBtnClaimIt()
    {

    }

    private void OnClickBtnNothanks()
    {
        Show(false);
    }
}
