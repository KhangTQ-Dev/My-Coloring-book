using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HG.Rate;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GalleryManager galleryManager;

    [SerializeField] private MyworkManager myworkManager;

    [SerializeField] private TabLobby tabLobbyGallery;

    [SerializeField] private TabLobby tabLobbyMywork;

    [SerializeField] private PopupManager popupManager;

    [SerializeField] private UiGamePlayManager uiGamePlayManager;

    [SerializeField] private Canvas canvasLobby;

    [SerializeField] private List<GraphicRaycaster> graphicRaycastersLobby;

    [SerializeField] private Canvas canvasGamePlay;

    [SerializeField] private RateManager rateManager;

    public Canvas CanvasLobby => canvasLobby;

    public Canvas CanvasGamePlay => canvasGamePlay;

    public PopupManager PopupManager => popupManager;

    public GalleryManager GalleryManager => galleryManager;

    public MyworkManager MyworkManager => myworkManager;

    public UiGamePlayManager UiGamePlayManager => uiGamePlayManager;

    public RateManager RateManager => rateManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeToGamePlay(TypeGallery typeGallery, TypeId typeId)
    {
        popupManager.ShowPopup(TypePopup.LoadingToGamePlay);

        StartCoroutine(WaitChangeToGamePlay());
    }

    public void OnChangeToGamePlayMywork(TypeGallery typeGallery, TypeId typeId)
    {
        popupManager.ShowPopup(TypePopup.PreviewMywork);

        StartCoroutine(WaitChangeToGamePlay());
    }

    IEnumerator WaitChangeToGamePlay()
    {
        yield return new WaitForSeconds(0);

        uiGamePlayManager.Init();

        canvasGamePlay.gameObject.SetActive(true);

        yield return new WaitForSeconds(0);

        for(int i = 0; i < graphicRaycastersLobby.Count; i++)
        {
            graphicRaycastersLobby[i].enabled = false;
        }

        canvasLobby.enabled = false;
    }

    public void OnChangeToLobby()
    {
        StartCoroutine(WaitChangeToLobby());
    }

    IEnumerator WaitChangeToLobby()
    {
        yield return new WaitForSeconds(0);

        canvasGamePlay.gameObject.SetActive(false);

        for (int i = 0; i < graphicRaycastersLobby.Count; i++)
        {
            graphicRaycastersLobby[i].enabled = true;
        }

        canvasLobby.enabled = true;

        ChangeTab(TabUi.Gallery);

        //yield return new WaitForSeconds(0.5f);


    }

    public void Init()
    {
        galleryManager.Show(true);
        myworkManager.Show(false);

        tabLobbyGallery.Show(true);
        tabLobbyMywork.Show(false);
    }

    public void ChangeTab(TabUi tabUi)
    {
        switch (tabUi)
        {
            case TabUi.Gallery:
                galleryManager.Show(true);
                myworkManager.Show(false);

                tabLobbyGallery.Show(true);
                tabLobbyMywork.Show(false);
                break;
            case TabUi.Mywork:
                galleryManager.Show(false);
                myworkManager.Show(true);

                tabLobbyGallery.Show(false);
                tabLobbyMywork.Show(true);
                break;
        }
    }
}
