using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GalleryManager galleryManager;

    [SerializeField] private MyworkManager myworkManager;

    [SerializeField] private TabLobby tabLobbyGallery;

    [SerializeField] private TabLobby tabLobbyMywork;

    public GalleryManager GalleryManager => galleryManager;

    public MyworkManager MyworkManager => myworkManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
