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
}