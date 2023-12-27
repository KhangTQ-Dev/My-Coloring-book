using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GamePlayManager gamePlayManager;

    [SerializeField] private UiManager uiManager;

    public GamePlayManager GamePlayManager => gamePlayManager;

    public UiManager UiManager => uiManager;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();

        Invoke("ShowBanner", 1.5f);

        Invoke("PlaySound", 1.5f);
    }

    private void PlaySound()
    {
        GameManager.Instance.SoundManager.PlaySoundBG();
    }

    private void ShowBanner()
    {
        AdsManager.Instance.ShowBanner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        uiManager.Init();
    }

    public void OnChangeToGamePlay(TypeGallery typeGallery, TypeId typeId)
    {
        gamePlayManager.Show(typeGallery, typeId);

        uiManager.OnChangeToGamePlay(typeGallery, typeId);
    }

    public void OnChangeToGamePlayMywork(TypeGallery typeGallery, TypeId typeId)
    {
        gamePlayManager.Show(typeGallery, typeId);

        uiManager.OnChangeToGamePlayMywork(typeGallery, typeId);
    }

    public void OnChangeToLobby()
    {
        gamePlayManager.Show(false);

        uiManager.OnChangeToLobby();
    }

    public void CheckShowPopupFree()
    {

    }
}