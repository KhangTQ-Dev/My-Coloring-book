using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Extensions;

#if UNITY_IOS
using System.Runtime.InteropServices;
namespace AudienceNetwork
{
    public static class AdSettings
    {
        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
    }
}

#endif
public enum EMRecPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    Centered,
    CenterLeft,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}
public class RevalueInfor
{
    [SerializeField] string sdk;
    [SerializeField] string network;
    [SerializeField] string id;
    [SerializeField] string placement;
    [SerializeField] double revenue;
    [SerializeField] string currency;

    public RevalueInfor()
    {
    }

    public RevalueInfor(string sdk, string network, string id, string placement, double revenue, string currency)
    {
        this.sdk = sdk;
        this.network = network;
        this.id = id;
        this.placement = placement;
        this.revenue = revenue;
        this.currency = currency;
    }

    public string Currency => currency;
    public double Revenue => revenue;
    public string Placement => placement;
    public string ID => id;
    public string SDK => sdk;
    public string Network => network;
}

public abstract class AdsManager : unity_base.Singleton<AdsManager>, IAOACallback
{
    [SerializeField] protected GoogleAD googleAD;

    [Header("Config")]
    [SerializeField] protected bool isRemoveAds;
    [SerializeField] protected bool isTestReward;

    [Header("Optional - Blank if not use")]
    [SerializeField] protected string mrecID = "";
    [SerializeField] protected string nativeID = "";
    [SerializeField] EMRecPosition mrecPosition = EMRecPosition.TopCenter;
    public EMRecPosition MRECPosition => mrecPosition;

    public void SetMRECPosition(EMRecPosition value)
    {
        mrecPosition = value;
    }
    [SerializeField] bool isMRECReady;
    public bool IsMRECReady => isMRECReady || (!FirebaseRemoteData.MREC_AD_ON_OFF);

    public void SetIsMRECReady(bool value)
    {
        isMRECReady = value;
    }

    public string AOA_ID => googleAD.AOAID;

    public string MREC_ID => mrecID;
    public string NATIVE_ID => nativeID;

    protected int openGameCount;
    protected int onPauseCount = 0;
    protected bool aoaShowing = false;


    #region Field
    public Action callbackReward = () => { };
    [SerializeField] bool aoaShowSuccess;
    public bool AoaShowSuccess => aoaShowSuccess;
    public void SetAoaShowSuccess(bool value)
    {
        aoaShowSuccess = value;
    }


    protected string placementReward = "";
    protected string placementInter = "";

    protected int interstitialRetryAttempt = 1;
    protected int rewardedRetryAttempt = 1;
    protected int bannerRetryAttempt = 0;
    protected int mrecRetryAttempt = 0;

    protected int aoaRetryAttempt = 0;
    protected int nativeRetryAttempt = 0;


    protected DateTime timeCloseIntersAds;
    protected DateTime timeOutGame;
    protected bool interShowing = false;
    protected bool rewardShowing = false;
    public bool IsRemoveAds => isRemoveAds;
    public void SetIsRemoveAds(bool value)
    {
        PlayerPrefs.SetInt("IsRemoveAds", value ? 1 : 0);
        isRemoveAds = value;
    }
    public bool IsPortrait => Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown || (Screen.orientation == ScreenOrientation.AutoRotation && (Screen.autorotateToPortrait || Screen.autorotateToPortraitUpsideDown));
    public bool IsConnectionNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        return true;
    }
    public int OpenGameCount => openGameCount;
    public void OnAdRevenuePaidEvent(RevalueInfor impressionData)
    {
        var impressionParameters = new[] {
        new Firebase.Analytics.Parameter("ad_platform", impressionData.SDK),
        new Firebase.Analytics.Parameter("ad_source", impressionData.Network),
        new Firebase.Analytics.Parameter("ad_unit_name", impressionData.ID),
        new Firebase.Analytics.Parameter("ad_format", impressionData.Placement), // Please check this - as we
        new Firebase.Analytics.Parameter("value", impressionData.Revenue),
        new Firebase.Analytics.Parameter("currency",impressionData.Currency), // All Applovin revenue is sent in USD
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);


        //AF
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("ad_format", impressionData.Placement);
        dic.Add("ad_unit_name", impressionData.ID);
        HandleAppsflyer.Instance.LogAdRevenue(impressionData.Network, impressionData.Revenue, impressionData.Currency, dic);

    }
    #endregion
    #region MonoBehaviour

    protected virtual void Start()
    {
        openGameCount = PlayerPrefs.GetInt("OpenGameCount", 0);
        PlayerPrefs.SetInt("OpenGameCount", openGameCount + 1);
    }
    #endregion
    #region Init
    public virtual void Init()
    {
        timeCloseIntersAds = DateTime.Now.AddDays(-1);
        timeOutGame = DateTime.Now.AddDays(-1);
        googleAD.SetAOAID(FirebaseRemoteData.OPEN_AD_ID);
        mrecID = FirebaseRemoteData.MREC_AD_ID;
        nativeID = FirebaseRemoteData.NATIVE_AD_ID;
        HandleFireBase.Instance.LogEventWithParameter("init_ads");
    }

    public virtual void OnSdkInitializedEvent()
    {
        InitInterstitialCallback();
        InitRewardedCallback();
        InitBannerCallback();
        InitMRECCallback();
    }

    #endregion
    #region Interstitial
    public abstract void InitInterstitialCallback();
    public abstract void LoadInterstitial();
    public abstract void ShowInterstitial(string placement, bool useCounter = true, bool ignoreCapping = false);
    public void OnInterstitialLoadedEvent()
    {
        interstitialRetryAttempt = 1;
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.AD_INTER_LOAD);
    }
    public void OnInterstitialLoadFailedEvent(int err)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke(nameof(LoadInterstitial), (float)retryDelay);
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.AD_INTER_LOAD_FAIL, new FirebaseParam("errormsg", err));
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTER_LOAD_FAIL);

    }
    public void OnInterstitialDisplayedEvent()
    {
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_DISPLAYED);
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.AD_INTER_SHOW, new FirebaseParam("placement", placementInter));
        Time.timeScale = 0;
    }
    public void OnInterstitialDisplayFailedEvent(int err)
    {
        ResetInterState();
        Time.timeScale = 1;
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.AD_INTER_SHOW_FAIL, new FirebaseParam("placement", placementInter), new FirebaseParam("errormsg", err));
        LoadInterstitial();

    }
    public void OnInterstitialClickedEvent()
    {
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.AD_INTER_CLICK, new FirebaseParam("placement", placementInter));

    }
    public void OnInterstitialHiddenEvent()
    {
        Invoke(nameof(ResetInterState), 0.5f);
        Time.timeScale = 1;
        timeCloseIntersAds = DateTime.Now;
        Invoke(nameof(LoadInterstitial), 0.5f);
    }
    void ResetInterState()
    {
        interShowing = false;
    }
    #endregion
    #region Rewarded
    public abstract void InitRewardedCallback();
    public abstract void LoadRewarded();
    public abstract void ShowRewarded(Action callback = null, string placement = "", Action onNoADS = null);
    public void OnRewardedLoadedEvent()
    {
        rewardedRetryAttempt = 1;
    }
    public abstract bool IsRewardedAdReady();
    public void OnRewardedLoadFailedEvent(int err)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Invoke(nameof(LoadRewarded), (float)retryDelay);
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.ADS_REWARD_LOAD_FAIL, new FirebaseParam("placement", placementReward), new FirebaseParam("errormsg", err));
        //HandleFireBase.Instance.LogEventWithParameterArray(HandleFireBase.ADS_REWARD_FAIL, new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter("placement", placement), new Firebase.Analytics.Parameter("errormsg", "RewardedFailedToLoad_" + err) });

    }
    public void OnRewardedDisplayFailedEvent(int err)
    {
        ResetRewardState();
        Time.timeScale = 1;
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.ADS_REWARD_SHOW_FAIL, new FirebaseParam("placement", placementReward), new FirebaseParam("errormsg", err));
        //HandleFireBase.Instance.LogEventWithParameterArray(HandleFireBase.ADS_REWARD_FAIL, new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter("placement", placement), new Firebase.Analytics.Parameter("errormsg", "RewardedFailedToDisplay_" + err) });
        LoadRewarded();
    }
    protected void ResetRewardState()
    {
        rewardShowing = false;
    }
    public void OnRewardedDisplayedEvent()
    {
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_REWARDED_DISPLAYED);
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.ADS_REWARD_SHOW, new FirebaseParam("placement", placementReward));
        Time.timeScale = 0;
    }
    public void OnRewardedClickedEvent()
    {
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.ADS_REWARD_CLICK, new FirebaseParam("placement", placementReward));

    }
    public void OnRewardedHiddenEvent()
    {
        Time.timeScale = 1;
        Invoke(nameof(ResetRewardState), 0.5f);
        timeCloseIntersAds = DateTime.Now;
        Invoke(nameof(LoadRewarded), 0.5f);
    }
    public void OnAdReceivedRewardEvent()
    {
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_REWARDED_AD_COMPLETED);
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.ADS_REWARD_COMPLETE, new FirebaseParam("placement", placementReward));
        timeCloseIntersAds = DateTime.Now;
        callbackReward?.Invoke();
    }
    public void OnRewardedOffer(string placementReward)
    {
        this.placementReward = placementReward;
        HandleFireBase.Instance.LogEventWithParameter(HandleFireBase.ADS_REWARD_OFFER, new FirebaseParam("placement", placementReward));
    }
    #endregion
    #region Banner
    public abstract void InitBannerCallback();
    public abstract void ShowMaxBanner();

    public void OnBannerLoadedEvent()
    {
        //bannerRetryAttempt = 0;
    }

    public void OnBannerLoadFailedEvent(int err)
    {
        //double retryDelay = Math.Pow(2, Math.Min(6, bannerRetryAttempt));
        //bannerRetryAttempt++;
        //Invoke("LoadBanner", (float)retryDelay);
    }
    public abstract void LoadBanner();
    public abstract void ShowBanner();
    public abstract void RefreshBanner();

    public abstract void HideBanner();
    #endregion
    #region AOA
    public abstract void InitAOACallback();
    public abstract void LoadAOA();
    public abstract void ShowAOA(bool ignoreCapping = false);
    public void OnAOALoadedEvent()
    {
        aoaRetryAttempt = 0;
    }

    public void OnAOAFullScreenContentOpened()
    {
    }

    public void OnAOAHiddenEvent()
    {
        SetAoaShowSuccess(true);
        aoaShowing = false;
        LoadAOA();
    }

    public void OnAOALoadFailedEvent(string err)
    {
        aoaShowing = false;
        aoaRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, aoaRetryAttempt));
        Invoke(nameof(LoadAOA), (float)retryDelay);
    }

    public void OnAOAShowFailedEvent(string err)
    {
        SetAoaShowSuccess(true);
        aoaShowing = false;
        aoaRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, aoaRetryAttempt));
        Invoke(nameof(LoadAOA), (float)retryDelay);
    }

    public void OnAOAPaidEvent(long value, string currencyCode)
    {
        OnAdRevenuePaidEvent(new RevalueInfor("Google Admob", "Google Admob", googleAD.AOAID, "Banner", value, currencyCode));
    }

    #endregion

    #region mrec
    public abstract void InitMRECCallback();

    public void OnMRECLoadedEvent()
    {
        mrecRetryAttempt = 0;
        SetIsMRECReady(true);
    }

    public void OnMRECLoadFailedEvent(int err)
    {
        double retryDelay = Math.Pow(2, Math.Min(6, mrecRetryAttempt));
        mrecRetryAttempt++;
        Invoke(nameof(LoadMREC), (float)retryDelay);
    }
    public abstract void LoadMREC();
    public abstract void ShowMREC();
    public abstract void HideMREC();
    public abstract void UpdateMRecPosition(EMRecPosition mrecPosition);

    public abstract void ShowFirstAOA();
    public abstract void ShowImageInter(Action callback);

    #endregion

    #region Native
    #endregion
    #region iOS
#if UNITY_IOS
    public void SetAdvertiserTrackingEnabled(bool value)
    {
        AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(value);
    }
#endif
    #endregion
}
