using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GamePlayManager gamePlayManager;

    [SerializeField] private UiManager uiManager;

    [SerializeField] private Transform parentAge;

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

        if (PlayerPrefs.GetInt("HG_AGE", 0) == 0)
        {
            // no hoi tuoi

            GameObject objLoadAge = Resources.Load<GameObject>("Age");

            Instantiate(objLoadAge, parentAge);
        }

        //

        float width = (float)Screen.width / (float)Screen.height;

        Debug.Log(width);

        //float k = Screen.width / 1080;

        //if (Screen.width != 1080)
        //{
        //    k = Camera.main.orthographicSize * 2 * k;
        //}

        float m = 1080.0f / 1920.0f;




        ////k = k / m;

        ////float y = Screen.height / 1920;

        ////float k = x <= y ? x : y;

        float z = width / m;

        //float a = x <= y ? x : y;

        transform.position = new Vector3(transform.position.x, transform.position.y * z, transform.position.z);
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