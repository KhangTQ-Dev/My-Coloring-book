using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Game;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GDPRScript gDPRScript;

    [SerializeField] private LoadingInitialGame loadingInitialGame;

    [SerializeField] private float timeWaitLoadInitial;

    [SerializeField] private float timeWaitAdsInit;

    [SerializeField] private float timeWaitFirebaseInit;

    [SerializeField] private int indexLobby;

    [SerializeField] private int indexIngame;

    [SerializeField] private UiGlobalManager uiGlobalManager;

    [SerializeField] private DataManager dataManager;

    [SerializeField] private SoundManager soundManager;

    [SerializeField] private VibrateManager vibrateManager;

    [SerializeField] private LoadingManager loadingManager;

    [SerializeField] private GameObject objWifi;

    public UiGlobalManager UiGlobalManager => uiGlobalManager;

    public DataManager DataManager => dataManager;

    public SoundManager SoundManager => soundManager;

    public VibrateManager VibrateManager => vibrateManager;

    public LoadingManager LoadingManager => loadingManager;

    public GDPRScript GDPRScript => gDPRScript;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CheckInternet", 0.75f);

        StartCoroutine(WaitLoadInitial());
    }

    IEnumerator WaitLoadInitial()
    {
        //InitSDK();

        yield return new WaitForSeconds(timeWaitFirebaseInit);

        InitFirebase();

        yield return new WaitForSeconds(timeWaitAdsInit);

        loadingInitialGame.OnPauseGDPR();

        InitAds();



        yield return new WaitUntil(() => gDPRScript.ShowGDPRPopupDone);



        loadingInitialGame.OnRemuseGDPR();



        yield return new WaitForSeconds(timeWaitLoadInitial);

        LoadInitialGame();
    }

    private void InitSDK()
    {
        Invoke("InitAds", timeWaitAdsInit);

        Invoke("InitFirebase", timeWaitFirebaseInit);
    }

    private void InitFirebase()
    {
        HandleFireBase.Instance.Initialize();
    }

    private void InitAds()
    {
        gDPRScript.CallGDPR();

        //AdsManager.Instance.Init();
    }

    private void InitAppflyer()
    {
        //HandleAppsflyer.Instance.i
    }

    public void LoadScene(int indexScene)
    {
        Resources.UnloadUnusedAssets();

        SceneManager.LoadScene(indexScene, LoadSceneMode.Single);
    }

    public void LoadLobby()
    {
        LoadScene(indexLobby);
    }

    public void LoadGame()
    {
        LoadScene(indexIngame);
    }

    public void LoadInitialGame()
    {
        LoadLobby();
    }

    public void CheckInternet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            objWifi.SetActive(false);

            //SetTimeScale(1);
        }
        else
        {
            objWifi.SetActive(true);

            //SetTimeScale(0);
        }

        Invoke("CheckInternet", 2);
    }
}