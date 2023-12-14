using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLogo : MonoBehaviour
{
    [SerializeField] private float timeLoad;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitLoadingLogo());
    }
    
    IEnumerator WaitLoadingLogo()
    {
        yield return new WaitForSeconds(timeLoad);

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void InitAds()
    {
        //AdsManager.Instance.Init();

        //Invoke("ShowBanner", 5);
    }

    private void ShowBanner()
    {
        //AdsManager.Instance.ShowBanner();
    }

    private void InitIap()
    {
        //HandleIAP.Instance.Initialize();
    }
}