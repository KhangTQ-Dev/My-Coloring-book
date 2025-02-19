using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGamePlayManager : MonoBehaviour
{
    [SerializeField] private UiBtnColorManager uiBtnColorManager;

    [SerializeField] private UiChooseColorManager uiChooseColorManager;

    [SerializeField] private UiColorPick uiColorPick;

    [SerializeField] private Button btnBack;

    [SerializeField] private Button btnBackAction;

    [SerializeField] private Button btnNextAction;

    [SerializeField] private Button btnEyeDrop;

    [SerializeField] private Button btnDone;

    [SerializeField] private Button btnChangeBar;

    [SerializeField] private Image imageRenderBtnPick;

    [SerializeField] private Image imageRenderBtnChoose;

    [SerializeField] private Image imageBack;

    [SerializeField] private Image imageNext;

    [SerializeField] private Image imageEyeDrop;

    [SerializeField] private Color colorCanInteract;

    [SerializeField] private Color colorCantInteract;

    public UiColorPick UiColorPick => uiColorPick;

    private TypeBarDraw typeBar;

    [SerializeField] private List<GameObject> objSmall;

    [SerializeField] private List<GameObject> objBig;

    private bool isActiveSmall;

    private bool isActiveBig;

    // Start is called before the first frame update
    void Start()
    {
        isActiveSmall = true;

        isActiveBig = true;

        btnBack.onClick.AddListener(OnClickBtnBack);
        btnBackAction.onClick.AddListener(OnClickBtnBackAction);
        btnNextAction.onClick.AddListener(OnClickBtnNextAction);
        btnEyeDrop.onClick.AddListener(OnClickBtnEyeDrop);
        btnDone.onClick.AddListener(OnClickBtnDone);
        btnChangeBar.onClick.AddListener(OnClickBtnChangeBar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        uiBtnColorManager.Init();
        uiChooseColorManager.Init();

        SetEyeDrop(true);

        SetBack(false);

        SetNext(false);

        ChangeBar(TypeBarDraw.pick);
    }

    private void SetBack(bool isTrue)
    {
        btnBackAction.enabled = isTrue;

        imageBack.color = isTrue ? colorCanInteract : colorCantInteract;
    }

    private void SetNext(bool isTrue)
    {
        btnNextAction.enabled = isTrue;

        imageNext.color = isTrue ? colorCanInteract : colorCantInteract;
    }

    public void OnClickBtnChoose(int idChoose)
    {
        uiBtnColorManager.OnClickBtnChoose(idChoose);
    }

    public void OnClickBtnChooseFollow(int idChoose)
    {
        uiBtnColorManager.OnClickBtnChooseFollow(idChoose);
    }

    private void OnClickBtnBack()
    {
        LevelManager.Instance.UiManager.PopupManager.ShowPopup(TypePopup.WarningLeave);
    }

    public void OnBack()
    {
        //bool a = GameManager.Instance.DataManager.GetFirstDone();

        //bool b = GameManager.Instance.DataManager.GetShowRate();

        //if (!a & !b)
        //{
        //    LevelManager.Instance.UiManager.RateManager.ShowPopup();

        //    GameManager.Instance.DataManager.SetFirstDone();

        //    GameManager.Instance.DataManager.SetShowRate();
        //}

        LevelManager.Instance.OnChangeToLobby();
    }

    private void OnClickBtnBackAction()
    {
        LevelManager.Instance.GamePlayManager.BackHistory();

        SetBackNext();
    }

    public void SetBackNext()
    {
        SetBack(LevelManager.Instance.GamePlayManager.CheckCanBackHistory());
        SetNext(LevelManager.Instance.GamePlayManager.CheckCanNextHistory());
    }

    private void OnClickBtnNextAction()
    {
        LevelManager.Instance.GamePlayManager.NextHistory();

        SetBackNext();
    }

    private void OnClickBtnEyeDrop()
    {
        LevelManager.Instance.GamePlayManager.Detect.SetCanEyeDrop();
    }

    private void OnClickBtnDone()
    {
        //bool a = GameManager.Instance.DataManager.GetFirstDone();

        //bool b = GameManager.Instance.DataManager.GetShowRate();

        //if(!a & !b)
        //{
        //    LevelManager.Instance.UiManager.RateManager.ShowPopup();

        //    GameManager.Instance.DataManager.SetFirstDone();

        //    GameManager.Instance.DataManager.SetShowRate();
        //}

        HandleFireBase.Instance.LogEventDrawComplete(LevelManager.Instance.GamePlayManager.GetCurrentTypeGallery(), LevelManager.Instance.GamePlayManager.GetCurrentTypeId());

        LevelManager.Instance.GamePlayManager.PictureManager.SavePicture();

        LevelManager.Instance.UiManager.PopupManager.ShowPopup(TypePopup.Preview);
    }

    private void OnClickBtnChangeBar()
    {
        if(typeBar == TypeBarDraw.pick)
        {
            ChangeBar(TypeBarDraw.Bar);
        }
        else
        {
            ChangeBar(TypeBarDraw.pick);
        }
    }

    public void ChangeBar(TypeBarDraw typeBarDraw)
    {
        switch (typeBarDraw)
        {
            case TypeBarDraw.pick:

                imageRenderBtnChoose.gameObject.SetActive(true);
                imageRenderBtnPick.gameObject.SetActive(false);

                uiBtnColorManager.Show(true);

                uiChooseColorManager.Show(false);

                break;
            case TypeBarDraw.Bar:

                imageRenderBtnChoose.gameObject.SetActive(false);
                imageRenderBtnPick.gameObject.SetActive(true);

                uiBtnColorManager.Show(false);

                uiChooseColorManager.Show(true);

                break;
        }

        typeBar = typeBarDraw;
    }

    public void SetEyeDrop(bool isTrue)
    {
        btnEyeDrop.enabled = isTrue;

        imageEyeDrop.color = isTrue ? colorCanInteract : colorCantInteract;
    }

    public void TurnSmallBtn()
    {
        isActiveSmall = !isActiveSmall;

        for(int i = 0; i < objSmall.Count; i++)
        {
            objSmall[i].SetActive(isActiveSmall);
        }
    }

    public void TurnBigBtn()
    {
        isActiveBig = !isActiveBig;

        for (int i = 0; i < objBig.Count; i++)
        {
            objBig[i].SetActive(isActiveBig);
        }
    }
}

public enum TypeBarDraw
{
    pick,
    Bar
}
